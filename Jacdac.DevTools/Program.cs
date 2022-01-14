using Jacdac;
using Jacdac.Transports.Spi;
using Microsoft.AspNetCore.Connections;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;

namespace Jacdac.DevTools
{
    public class ProxyTransport : Transport
    {
        public delegate void SendFrameHandler(byte[] data);
        private SendFrameHandler sendFrame;

        public ProxyTransport(SendFrameHandler sendFrame)
            : base("devtools")
        {
            this.sendFrame = sendFrame;
        }

        public override event FrameEventHandler? FrameReceived;
        public override event TransportErrorReceivedEvent? ErrorReceived;

        public void RaiseFrameReceived(byte[] frame)
        {
            this.FrameReceived?.Invoke(this, frame);
        }

        public override void SendFrame(byte[] data)
        {
            this.sendFrame(data);
        }

        protected override void InternalConnect()
        {
            this.SetConnectionState(ConnectionState.Connected);
        }

        protected override void InternalDisconnect()
        {
            this.SetConnectionState(ConnectionState.Disconnected);
        }
    }

    public static class Program
    {
        public static void Main(string[] args)
        {
            AsyncMain(args).Wait();
        }

        static async Task AsyncMain(string[] args)
        {
            var internet = args.Any(arg => arg == "--internet");
            var spi = args.Any(arg => arg == "--spi");
            var stats = args.Any(arg => arg == "--stats");
            var host = internet ? "*" : "localhost";
            var port = 8081;
            var url = $"http://{host}:{port}";

            Console.WriteLine("Jacdac DevTools (.NET)");
            Console.WriteLine("");
            Console.WriteLine("  --spi       enable SPI transport");
            Console.WriteLine("  --internet  bind all network interfaces");
            Console.WriteLine("  --stats     show various stats");
            Console.WriteLine("");
            Console.WriteLine($"   dashboard: {url}");
            Console.WriteLine($"   websocket: ws://{host}:{port}");
            if (internet)
            {
                Console.WriteLine("WARNING: all network interfaces bound");
                var server = Dns.GetHostName();
                var heserver = Dns.GetHostEntry(server);
                foreach (var ip in heserver.AddressList)
                    Console.WriteLine($"  {ip}");
            }

            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // better concurrent data structure?
            var clients = new List<WebSocket>();
            void SendFrame(WebSocket[] cs, byte[] frame)
            {
                if (cs.Length == 0) return;
                Task.WaitAll(
                    cs
                    .Select(async (client) =>
                    {
                        try
                        {
                            await client.SendAsync(frame, WebSocketMessageType.Binary, true, CancellationToken.None);
                        }
                        catch
                        {
                            lock (clients)
                                clients.Remove(client);
                            try
                            {
                                client?.Dispose();
                            }
                            catch { }
                        }
                    }).ToArray());
            }

            ProxyTransport? proxyTransport = null;
            if (spi)
            {
                Console.WriteLine($"{DateTime.Now}> jacdac: starting bus...");
                var spiTransport = SpiTransport.Create();
                proxyTransport = new ProxyTransport(frame =>
                {
                    WebSocket[] cs;
                    lock (clients)
                        cs = clients.ToArray();
                    SendFrame(cs, frame);
                });
                var bus = new JDBus(spiTransport, new JDBusOptions
                {
                    IsClient = false,
                    IsProxy = true,
                    IsInfrastructure = true,
                    DisableUniqueBrain = true,
                    DisableRoleManager = true,
                    DisableLogger = true,
                });
                bus.AddTransport(proxyTransport);
                bus.DeviceConnected += (s, e) =>
                {
                    Console.WriteLine($"{DateTime.Now}> device connected: {e.Device}");
                    e.Device.Restarted += (s2, e2) => Console.WriteLine($"{DateTime.Now}> device restarted: {e.Device}");
                };
                bus.DeviceDisconnected += (sender, device) => Console.WriteLine($"{DateTime.Now}> device disconnected: {device.Device}");
                if (stats)
                {
                    new Timer(state =>
                    {
                        Console.Write(bus.Describe());
                    }, null, 0, 30000);
                }
            }

            // download proxy code
            var resp = await new HttpClient().GetAsync("https://microsoft.github.io/jacdac-docs/devtools/proxy");
            resp.EnsureSuccessStatusCode();
            var proxySource = await resp.Content.ReadAsStringAsync();

            app.Lifetime.ApplicationStopping.Register(() =>
            {
                lock (clients)
                {
                    foreach (var client in clients)
                    {
                        try
                        {
                            client.Dispose();
                        }
                        catch { }
                    }
                    clients.Clear();
                }
            });
            app.UseWebSockets();
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/" && context.WebSockets.IsWebSocketRequest)
                {
                    var ws = await context.WebSockets.AcceptWebSocketAsync();
                    lock (clients)
                    {
                        clients.Add(ws);
                        Console.WriteLine($"{DateTime.Now}> clients: {clients.Count} connected");
                    }
                    var proxy = async () =>
                        {
                            try
                            {
                                var buffer = new byte[512];
                                while (ws.State == WebSocketState.Open)
                                {
                                    // grab frame
                                    var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                                    var frame = buffer.Take(result.Count).ToArray();
                                    // dispatch to bus
                                    proxyTransport?.RaiseFrameReceived(frame);
                                    // dispatch to other clients
                                    WebSocket[] cs;
                                    lock (clients)
                                        cs = clients.Where(client => client != ws).ToArray();
                                    SendFrame(cs, frame);
                                }
                            }
                            catch (SocketException)
                            {

                            }
                            finally
                            {
                                // web socket closed, clean
                                lock (clients)
                                {
                                    clients.Remove(ws);
                                    Console.WriteLine($"{DateTime.Now}> clients: {clients.Count} connected");
                                }
                                try
                                {

                                }
                                catch
                                {
                                    ws.Dispose();
                                }
                            }
                        };
                    await proxy();
                }
                else if (context.Request.Path == "/")
                {
                    context.Response.Headers.ContentType = "text/html";
                    context.Response.Headers.CacheControl = "no-cache";
                    await context.Response.WriteAsync(proxySource);
                    await context.Response.CompleteAsync();
                }
                else
                {
                    await next();
                }

            });
            app.Run(url);

        }
    }
}