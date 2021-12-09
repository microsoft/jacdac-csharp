using System;
using System.Runtime.CompilerServices;
using GHIElectronics.TinyCLR.Devices.Uart;
using GHIElectronics.TinyCLR.Native;
using Jacdac;

namespace GHIElectronics.TinyCLR.Devices.Jacdac {

    public class JacdacSetting {
        public TimeSpan StartPulseDuration { get; set; } = TimeSpan.FromTicks(140);
        public TimeSpan StartDataGapDuration { get; set; } = TimeSpan.FromTicks(600);
        public TimeSpan DataByteGapDuration { get; set; } = TimeSpan.FromTicks(0);
        public TimeSpan DataEndGapDuration { get; set; } = TimeSpan.FromTicks(0);
        public TimeSpan EndPulseDuration { get; set; } = TimeSpan.FromTicks(140);
        public TimeSpan FrameToFramDuration { get; set; } = TimeSpan.FromTicks(200);

        public int WriteBufferSize => 1;
        public int ReadBufferSize { get; set; } = 20;
    }

    public sealed class UARTTransport : ITransport {

        static UARTTransport() {
            global::Jacdac.Platform.Crc16 = NativeCrc;
        }

        private int uartController;
        private bool swapTxRx;
        private JacdacSetting jacdacSetting;
        private UartSetting uartSetting;
        private bool disposed;
        private ConnectionState connectionState = ConnectionState.Disconnected;

        private readonly NativeEventDispatcher nativeReceivedEventDispatcher;
        private readonly NativeEventDispatcher nativeErrorEventDispatcher;

        private PacketReceivedEvent packetReceived;
        private TransportErrorReceivedEvent errorReceived;

        public UARTTransport(string uartPortName) : this(uartPortName, null) {

        }
        public UARTTransport(string uartPortName, UartSetting uartSetting) : this(uartPortName, uartSetting, null) {

        }
        public UARTTransport(string uartPortName, UartSetting uartSetting, JacdacSetting jacdacSetting) {

            this.uartSetting = uartSetting ?? new UartSetting() {
                BaudRate = 1000000,
            };

            this.jacdacSetting = jacdacSetting ?? new JacdacSetting();

            if (this.uartSetting.BaudRate == 0)
                this.uartSetting.BaudRate = 1000000;

            if (this.uartSetting.DataBits == 0)
                this.uartSetting.DataBits = 8;

            if (this.uartSetting.BaudRate != 1000000
                || this.uartSetting.DataBits != 8
                || this.uartSetting.Parity != UartParity.None
                || this.uartSetting.StopBits != UartStopBitCount.One
                || this.uartSetting.Handshaking != UartHandshake.None
                  )

                throw new Exception("Not support.");

            if (this.jacdacSetting.ReadBufferSize < 1)
                throw new Exception("Not support.");


            this.swapTxRx = this.uartSetting.SwapTxRxPin;

            this.uartController = int.Parse(uartPortName.Substring(uartPortName.Length - 1, 1));

            this.Acquire();

            this.nativeReceivedEventDispatcher = NativeEventDispatcher.GetDispatcher("GHIElectronics.TinyCLR.NativeEventNames.JacdacController.RecivedEvent");
            this.nativeErrorEventDispatcher = NativeEventDispatcher.GetDispatcher("GHIElectronics.TinyCLR.NativeEventNames.JacdacController.ErrorEvent");

            this.nativeReceivedEventDispatcher.OnInterrupt += (apiName, d0, d1, d2, d3, ts) => {
                if (apiName.CompareTo("JacdacController") == 0 && d0 == this.uartController) {
                    var time = TimeSpan.FromTicks(d2);
                    var data = new byte[(int)d1];

                    this.NativeReadRawPacket(this.uartController, data, data.Length);

                    var packet = Packet.FromBinary(data);

                    packet.Timestamp = time;

                    this.packetReceived?.Invoke(this, packet);
                }
            };

            this.nativeErrorEventDispatcher.OnInterrupt += (apiName, d0, d1, d2, d3, ts) => {
                if (apiName.CompareTo("JacdacController") == 0 && d0 == this.uartController) {
                    byte[] data = null;

                    var error = (TransportError)d2;


                    if (d1 > 0) {
                        data = new byte[(int)d1];

                        this.NativeReadErrorRawPacket(this.uartController, data, data.Length);
                    }

                    var args = new TransportErrorReceivedEventArgs(error, ts, data);

                    this.errorReceived?.Invoke(this, args);


                };

            };
        }

        private void SetConnectionState(ConnectionState connectionState)
        {
            if (connectionState != this.connectionState)
            {
                this.connectionState = connectionState;
                this.ConnectionStateChanged.Invoke(this, connectionState);
            }
        }

        private void Acquire() => this.NativeAcquire(this.uartController);

        private void Release() {
            this.Disconnect();
            this.NativeRelease(this.uartController);
        }

        public void Connect() {
            if (this.connectionState == ConnectionState.Connected)
            {
                // already connected
                return;
            }

            this.SetConnectionState(ConnectionState.Connecting);
            var startPulseDuration = this.jacdacSetting.StartPulseDuration.Ticks;
            var startDataGapDuration = this.jacdacSetting.StartDataGapDuration.Ticks;
            var dataByteGapDuration = this.jacdacSetting.DataByteGapDuration.Ticks;
            var dataEndGapDuration = this.jacdacSetting.DataEndGapDuration.Ticks;
            var endPulseDuration = this.jacdacSetting.EndPulseDuration.Ticks;
            var frameToFramDuration = this.jacdacSetting.FrameToFramDuration.Ticks;
            var readBufferSize = this.jacdacSetting.ReadBufferSize;

            this.NativeSetActiveSetting(this.uartController, this.swapTxRx, startPulseDuration, startDataGapDuration, dataByteGapDuration, dataEndGapDuration, endPulseDuration, frameToFramDuration, readBufferSize);
            this.NativeEnable(this.uartController);

            this.SetConnectionState(ConnectionState.Connected);
        }

        public void Disconnect()
        {
            if (this.connectionState == ConnectionState.Disconnected)
            {
                // already connected
                return;
            }

            this.SetConnectionState(ConnectionState.Disconnecting);
            this.NativeDisable(this.uartController);
            this.SetConnectionState(ConnectionState.Disconnected);
        }

        public void SendPacket(global::Jacdac.Packet packet) {
            var data = new byte[packet.Header.Length + packet.Data.Length];

            Array.Copy(packet.Header, data, packet.Header.Length);
            Array.Copy(packet.Data, 0, data, packet.Header.Length, packet.Data.Length);

            data[2] = (byte)(packet.Size + 4);

            var crc = Util.CRC(data, 2, data.Length - 2);

            data[0] = (byte)(crc >> 0);
            data[1] = (byte)(crc >> 8);

            this.SendData(data);
        }

        private void SendData(byte[] data) => this.SendData(data, 0, data.Length);
        private void SendData(byte[] data, int offset, int count) {
            if (data == null
                || offset < 0
                || offset + count > data.Length) {
                throw new ArgumentException("Invalid argument.");
            }
            this.NativeSendRawPacket(this.uartController, data, offset, count);
        }

        public event ConnectionStateChangedEvent ConnectionStateChanged;

        public event PacketReceivedEvent PacketReceived {
            add {
                if (this.packetReceived == null) {
                    this.NativeEnableReceivedPacketEvent(this.uartController, true);
                }
                this.packetReceived += value;
            }

            remove {
                if (this.packetReceived != null) {
                    this.packetReceived -= value;
                }

                if (this.packetReceived == null) {
                    this.NativeEnableReceivedPacketEvent(this.uartController, false);
                }
            }
        }

        public event TransportErrorReceivedEvent ErrorReceived {
            add {
                if (this.errorReceived == null) {
                    this.NativeEnableErrorReceivedEvent(this.uartController, true);
                }
                this.errorReceived += value;
            }

            remove {
                if (this.errorReceived != null) {
                    this.errorReceived -= value;
                }

                if (this.errorReceived == null) {
                    this.NativeEnableErrorReceivedEvent(this.uartController, false);
                }
            }
        }

        public void ClearReadBuffer() => this.NativeClearReadBuffer(this.uartController);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeAcquire(int uartController);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeRelease(int uartController);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeSetActiveSetting(int uartController, bool swapTxRx, long startPulseDuration, long startDataGapDuration, long dataByteGapDuration, long dataEndGapDuration, long endPulseDuration, long frameToFramDuration, int readBufferSize);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeEnable(int uartController);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeDisable(int uartController);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeSendRawPacket(int uartController, byte[] data, int offset, int count);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int NativeReadRawPacket(int uartController, byte[] data, int count);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeEnableReceivedPacketEvent(int uartController, bool value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeEnableErrorReceivedEvent(int uartController, bool value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeClearReadBuffer(int uartController);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int NativeReadErrorRawPacket(int uartController, byte[] data, int count);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern ushort NativeCrc(byte[] data, int offset, int cout);

        public void Dispose() {
            if (!this.disposed) {
                this.disposed = true;

                this.Release();

            }
        }
    }


}
