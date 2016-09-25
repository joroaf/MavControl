﻿/*
The MIT License (MIT)

Copyright (c) 2013, David Suarez

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Concurrent;

namespace MavLinkNet
{
    public class MavLinkUdpTransport : MavLinkGenericTransport
    {
        public int UdpListeningPort = 14550;
        private IPEndPoint ep;
        public int HeartBeatUpdateRateMs = 1000;

        public event EventHandler<EventArgs> ConnectionEstablished;

        private ConcurrentQueue<byte[]> mReceiveQueue = new ConcurrentQueue<byte[]>();
        private ConcurrentQueue<UasMessage> mSendQueue = new ConcurrentQueue<UasMessage>();
        private AutoResetEvent mReceiveSignal = new AutoResetEvent(true);
        private AutoResetEvent mSendSignal = new AutoResetEvent(true);
        private AutoResetEvent mRemoteClientSignal = new AutoResetEvent(false);
        private MavLinkAsyncWalker mMavLink = new MavLinkAsyncWalker();
        private UdpClient mUdpClient;
        private bool mIsActive = true;
        private bool mReceiving = true;

        public override void Initialize()
        {
            ep = new IPEndPoint(IPAddress.Any, UdpListeningPort);

            InitializeMavLink();
            InitializeUdpListener();
            ThreadPool.QueueUserWorkItem(
                new WaitCallback(InitializeUdpSender), null);
        }

        public override void Dispose()
        {
            mReceiving = false;
            mIsActive = false;
            mUdpClient.Close();
            mReceiveSignal.Set();
            mSendSignal.Set();
        }

        private void InitializeMavLink()
        {
            mMavLink.PacketReceived += HandlePacketReceived;
            mMavLink.PacketReceived += FirstMAVlinkPacketReceived;
        }

        private void InitializeUdpListener()
        {
            // Create UDP listening socket on port
            mUdpClient = new UdpClient(ep);

            mUdpClient.BeginReceive(
                new AsyncCallback(ReceiveCallback), ep);

            ThreadPool.QueueUserWorkItem(
                new WaitCallback(ProcessReceiveQueue), null);
        }

        private void InitializeUdpSender(object state)
        {
            mRemoteClientSignal.WaitOne();
            ThreadPool.QueueUserWorkItem(
               new WaitCallback(ProcessSendQueue), ep);
        }


        // __ Receive _________________________________________________________


        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                ep = ar.AsyncState as IPEndPoint;

                if (mReceiving)
                {
                    mReceiveQueue.Enqueue(mUdpClient.EndReceive(ar, ref ep));
                }

                if (!mIsActive)
                {
                    mReceiveSignal.Set();
                    return;
                }

                mUdpClient.BeginReceive(new AsyncCallback(ReceiveCallback), ar);

                // Signal processReceive thread
                mReceiveSignal.Set();
            }
            catch (SocketException)
            {
                mIsActive = false;
            }
        }

        private void ProcessReceiveQueue(object state)
        {
            while (true)
            {
                byte[] buffer;

                if (mReceiveQueue.TryDequeue(out buffer))
                {
                    mMavLink.ProcessReceivedBytes(buffer, 0, buffer.Length);
                }
                else
                {
                    // Empty queue, sleep until signalled
                    mReceiveSignal.WaitOne();

                    if (!mIsActive) break;
                }
            }

            HandleReceptionEnded(this);
        }


        // __ Send ____________________________________________________________


        private void ProcessSendQueue(object state)
        {
            while (true)
            {
                UasMessage msg;

                if (mSendQueue.TryDequeue(out msg))
                {
                    SendMavlinkMessage(state as IPEndPoint, msg);
                }
                else
                {
                    // Queue is empty, sleep until signalled
                    mSendSignal.WaitOne();

                    if (!mIsActive) break;
                }
            }
        }

        private void SendMavlinkMessage(IPEndPoint ep, UasMessage msg)
        {
            byte[] buffer = mMavLink.SerializeMessage(msg, MavlinkSystemId, MavlinkComponentId, true);

            mUdpClient.Send(buffer, buffer.Length, ep);
        }


        // __ Heartbeat _______________________________________________________


        public void BeginHeartBeatLoop()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(HeartBeatLoop), null);
        }

        private void HeartBeatLoop(object state)
        {
            while (true)
            {
                foreach (UasMessage m in UavState.GetHeartBeatObjects())
                {
                    SendMessage(m);
                }

                Thread.Sleep(HeartBeatUpdateRateMs);
            }
        }

        private void FirstMAVlinkPacketReceived(object sender, MavLinkPacket e)
        {
            mMavLink.PacketReceived -= FirstMAVlinkPacketReceived;
            mRemoteClientSignal.Set();
            OnConnectionEstablished(EventArgs.Empty);
            mRemoteClientSignal.Dispose();
        }

        protected virtual void OnConnectionEstablished(EventArgs e)
        {
            EventHandler<EventArgs> handler = ConnectionEstablished;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        // __ API _____________________________________________________________


        public override void SendMessage(UasMessage msg)
        {
            mSendQueue.Enqueue(msg);

            // Signal send thread
            mSendSignal.Set();
        }
    }
}
