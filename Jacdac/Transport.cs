﻿using System;

namespace Jacdac
{
    public delegate void ConnectionStateChangedEvent(Transport sender, ConnectionState newState);

    public delegate void FrameReceivedEvent(Transport sender, byte[] frame, TimeSpan timestamp);

    public enum TransportError
    {
        Frame = 0,
        Overrun = 1,
        BufferFull = 2,
    }

    public sealed class TransportErrorReceivedEventArgs
    {
        public TransportError Error { get; }
        public DateTime Timestamp { get; }

        public byte[] Data { get; }

        public TransportErrorReceivedEventArgs(TransportError error, DateTime timestamp, byte[] data)
        {
            this.Error = error;
            this.Timestamp = timestamp;
            this.Data = data;
        }
    }

    public delegate void TransportErrorReceivedEvent(Transport sender, TransportErrorReceivedEventArgs args);

    public enum ConnectionState
    {
        Connected,
        Connecting,
        Disconnecting,
        Disconnected,
    }

    public abstract class Transport : JDNode, IDisposable
    {
        private ConnectionState _connectionState = ConnectionState.Disconnected;

        protected Transport()
        {

        }

        protected void SetConnectionState(ConnectionState connectionState)
        {
            if (this._connectionState != connectionState)
            {
                this._connectionState = connectionState;
                if (null != this.ConnectionChanged)
                    this.ConnectionChanged.Invoke(this, this._connectionState);
            }
        }

        public ConnectionState ConnectionState
        {
            get { return this._connectionState; }
        }

        public void SendPacket(Packet packet)
        {
            var data = new byte[packet.Header.Length + packet.Data.Length];
            packet.Header.CopyTo(data, 0);
            packet.Data.CopyTo(data, packet.Header.Length);
            data[2] = (byte)(packet.Size + 4);

            var crc = Util.CRC(data, 2, data.Length - 2);

            data[0] = (byte)(crc >> 0);
            data[1] = (byte)(crc >> 8);

            this.SendData(data);
        }

        /// <summary>
        /// Sends data over the transport
        /// </summary>
        /// <param name="packet"></param>
        protected abstract void SendData(byte[] data);

        /// <summary>
        /// Connect to the transport
        /// </summary>
        public void Connect()
        {
            if (this._connectionState != ConnectionState.Disconnected)
                return;

            this.SetConnectionState(ConnectionState.Connecting);
            try
            {
                this.InternalConnect();
                this.SetConnectionState(ConnectionState.Connected);
            }
            catch (Exception)
            {
                this.SetConnectionState(ConnectionState.Disconnected);
                throw;
            }
        }

        protected abstract void InternalConnect();

        /// <summary>
        /// Disconnects from the transport
        /// </summary>
        public void Disconnect()
        {
            if (this._connectionState != ConnectionState.Connected)
                return;

            this.SetConnectionState(ConnectionState.Disconnecting);
            try
            {
                this.InternalDisconnect();
                this.SetConnectionState(ConnectionState.Disconnected);
            }
            catch (Exception)
            {
                this.SetConnectionState(ConnectionState.Disconnected);
                throw;
            }
        }

        protected abstract void InternalDisconnect();

        public virtual void Dispose() { }

        public abstract event FrameReceivedEvent FrameReceived;

        /// <summary>
        /// Raised when an error is received
        /// </summary>
        public abstract event TransportErrorReceivedEvent ErrorReceived;

        /// <summary>
        /// Raised when the connection state of the transport changed
        /// </summary>
        public event ConnectionStateChangedEvent ConnectionChanged;
    }
}