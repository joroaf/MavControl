using MavLinkNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MavControl
{
    public delegate void UpdateAttitudeDelegate(object sender, UasAttitude message);
    public delegate void UpdateHUDDelegate(object sender, UasVfrHud message);
    public delegate void UpdateStatusDelegate(object sender, UasStatustext message);
    public delegate void HeartBeatDelegate(object sender, UasHeartbeat message);

    class MavControlBase
    {
        public const byte MAV_SYS_ID = 0xff;
        public const byte MAV_COMP_ID = 0xbe;
        public const byte MAV_TARGET_SYS_ID = 0x01;
        public const byte MAV_TARGET_COMP_ID = 0x01;

        public event EventHandler<LogEventArgs> TwoWayConnection;
        public event UpdateAttitudeDelegate UpdateAttitude;
        public event UpdateHUDDelegate UpdateHUD;
        public event UpdateStatusDelegate UpdateStatus;
        public event HeartBeatDelegate HeartBeat;

        protected LogEventArgs LogArgs = new LogEventArgs();

        protected virtual void OnTwoWayConnection()
        {
            EventHandler<LogEventArgs> h = TwoWayConnection;
            if (h != null)
            {
                LogArgs.msg = "Got UDP connection with QUAD.";
                h(this, LogArgs);
            }
        }

        protected virtual void OnUpdateAttitude(UasAttitude message)
        {
            if (message == null) return;

            UpdateAttitude(this, message);
        }

        protected virtual void OnUpdateHUD(UasVfrHud message)
        {
            if (message == null) return;

            UpdateHUD(this, message);
        }

        protected virtual void OnUpdateStatus(UasStatustext message)
        {
            if (message == null) return;

            UpdateStatus(this, message);
        }

        protected virtual void OnHeartBeat(UasHeartbeat message)
        {
            if (message == null) return;

            HeartBeat(this, message);
        }
    }
}
