﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpLib
{

    public class UdpMessageEventArg : EventArgs
    {
        public UdpMessageEventArg(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }

    public class UDPSocket
    {
        private readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private const int bufSize = 8 * 1024;
        private readonly State state = new State();
        private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
        private AsyncCallback recv = null;

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }

        public void Server(string address, int port)
        {
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            Receive();
        }

        public void Client(string address, int port)
        {
            _socket.Connect(IPAddress.Parse(address), port);
            Receive();
        }

        public void Send(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            _socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndSend(ar);
             //   Console.WriteLine("SEND: {0}, {1}", bytes, text);
            }, state);
        }

        protected void OnRecive(UdpMessageEventArg e)
        {
            EventHandler<UdpMessageEventArg> handler = MessageRecived;
            handler?.Invoke(this, e);

        }

        public EventHandler<UdpMessageEventArg> MessageRecived;

        private void Receive()
        {
            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
                _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);

                var message = Encoding.UTF8.GetString(so.buffer);
                OnRecive(new UdpMessageEventArg(message));
                //Console.WriteLine("RECV: {0}: {1}, {2}", epFrom.ToString(), bytes, Encoding.ASCII.GetString(so.buffer, 0, bytes));


            }, state);
        }
    }
}