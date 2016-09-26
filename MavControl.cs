using System;
using System.Collections.Generic;
using System.Net.Sockets;
using MavLinkNet;
using System.Threading.Tasks;
using System.Threading;

namespace MavControl
{
    class MavControl : MavControlBase
    {
        private UasRcChannelsOverride RcOverrideUasMsg;
        private MavLinkUdpTransport UdpServer;

        private bool udpSenderInitiated = false;

        private UasVfrHud HudData;
        private UasAttitude Attitude;
        private UasStatustext StatusText;
        private UasHeartbeat HeartBeatMsg;
        private JoystickDataEventArgs args;

        public MavControl()
        {
            RcOverrideUasMsg = new UasRcChannelsOverride();
            RcOverrideUasMsg.TargetSystem = MAV_TARGET_SYS_ID;
            RcOverrideUasMsg.TargetComponent = MAV_TARGET_COMP_ID;

            UdpServer = new MavLinkUdpTransport();
            UdpServer.ConnectionEstablished += InitUdpSender;

            UdpServer.MavlinkSystemId = MAV_SYS_ID;
            UdpServer.MavlinkComponentId = MAV_COMP_ID;
            UdpServer.OnPacketReceived += OnPacketReceived;

            UdpServer.Initialize();
        }

        public void Dispose()
        {
            UdpServer.Dispose();
        }

        public void sendJoysticInput(Object sender, JoystickDataEventArgs args)
        {
            ThreadPool.QueueUserWorkItem(prepareAndSendJoystickData, args);
        }

        private void prepareAndSendJoystickData(object state)
        {
            args = (JoystickDataEventArgs) state;

            RcOverrideUasMsg.Chan1Raw = args.jInput[1]; //roll
            RcOverrideUasMsg.Chan2Raw = args.jInput[2]; //pitch
            RcOverrideUasMsg.Chan3Raw = args.jInput[0]; //throttle
            RcOverrideUasMsg.Chan4Raw = args.jInput[3]; //yaw
            RcOverrideUasMsg.Chan6Raw = args.jInput[4]; //chan5

            Send(RcOverrideUasMsg);
        }

        private void Send(UasMessage m)
        {
            if (udpSenderInitiated)
            {
                UdpServer.SendMessage(m);
            }
        }

        private void InitUdpSender(Object s, EventArgs e)
        {
            OnTwoWayConnection();
            try
            {
                UdpServer.BeginHeartBeatLoop();
                udpSenderInitiated = true;
            }
            catch (SocketException)
            {
                //TODO
            }
        }

        private async void OnPacketReceived(object sender, MavLinkPacket packet)
        {
            await Task.Run(new Action(() =>
            {
                if (packet.IsValid)
                {
                    switch (packet.MessageId)
                    {
                        case 0x1E: //UasAttitude
                            Attitude = (UasAttitude)packet.Message;
                            OnUpdateAttitude(Attitude);
                            break;
                        case 0x4A: //UasVfrHud
                            HudData = (UasVfrHud)packet.Message;
                            OnUpdateHUD(HudData);
                            break;
                        case 0xFD:
                            StatusText = (UasStatustext)packet.Message;
                            OnUpdateStatus(StatusText);
                            break;
                        case 0x00:
                            HeartBeatMsg = (UasHeartbeat)packet.Message;
                            OnHeartBeat(HeartBeatMsg);
                        //TODO: 
                        //Heardbeat
                        //Restart timer.. if no packet for 1 sec => quad is lost connection
                        //ENDTODO
                            break;
                        default:
                            break;
                    }
                }
            }));
        }
    }
}
