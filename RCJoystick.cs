using System;
using SlimDX.DirectInput;
using System.Threading;
using System.Collections.Generic;

namespace MavControl
{
    class RCJoystick
    {
        /// <summary>
        /// Constants
        /// </summary>
        const ushort PWM_MIDDLE = 1500;
        const int THROTTLE_RATIO = 64; 
        const int ROLL_RATIO = 64; 
        const int PITCH_RATIO = 64; 
        const int YAW_RATIO = 64; 
        private int[] JOYSTICK_INPUT_RANGE = { -127, 127 };

        /// 
        /// Joystick handle
        /// 
        private Joystick Joystick;
        public JoystickState State;

        private bool jPolling = true;

        private int throttle;
        private int roll;
        private int pitch;
        private int yaw;

        private ushort[] joystickInput = new ushort[5];

        public event EventHandler<JoystickDataEventArgs> NewJoystickData;
        public event EventHandler<LogEventArgs> JoystickFailed;

        public Dictionary<string, ushort> FlightModes;
        public Dictionary<int, string> ModeFlights;
        private bool[] btns;

        /// 
        /// Construct, attach the joystick
        /// 
        public RCJoystick()
        {
            DirectInput dinput = new DirectInput();

            // Search for device
            foreach (DeviceInstance device in dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                // Create device
                try
                {
                    Joystick = new Joystick(dinput, device.InstanceGuid);
                    break;
                }
                catch (DirectInputException)
                {
                }
            }

            if (Joystick == null)
                throw new JoystickException("No joystick found");

            foreach (DeviceObjectInstance deviceObject in Joystick.GetObjects())
            {
                if ((deviceObject.ObjectType & ObjectDeviceType.Axis) != 0)
                    Joystick.GetObjectPropertiesById((int)deviceObject.ObjectType).SetRange(JOYSTICK_INPUT_RANGE[0], JOYSTICK_INPUT_RANGE[1]);
            }

            FlightModes = new Dictionary<string, ushort>()
            {
                {"Loiter", 1000},
                {"Stabilize", 1300},
                {"AltHold", 1400},
                {"PosHold", 1550}
            };

            ModeFlights = new Dictionary<int, string>()
            {
                {1000, "Stabilize"},
                {1300, "Loiter"},
                {1400, "AltHold"},
                {1550, "PosHold"}
            };

            // Acquire sdevice
            Joystick.Acquire();

            ThreadPool.QueueUserWorkItem(getJoystickInput);
        }

        private void getJoystickInput(object state)
        {
            JoystickDataEventArgs args = new JoystickDataEventArgs();

            try
            {
                joystickInput[4] = 1000;

                while (jPolling)
                {
                    if (Joystick.Acquire().IsFailure)
                        throw new JoystickException("Joystick failure");

                    if (Joystick.Poll().IsFailure)
                        throw new JoystickException("Joystick failure");

                    State = Joystick.GetCurrentState();

                    //Get raw values
                    throttle = State.GetSliders()[0];
                    roll = State.X;
                    pitch = State.Y;
                    yaw = State.RotationZ;
                    btns = State.GetButtons();

                    //throttle
                    joystickInput[0] = smoothValue(-throttle, THROTTLE_RATIO);

                    //roll
                    joystickInput[1] = smoothValue(roll, ROLL_RATIO);

                    //pitch
                    joystickInput[2] = smoothValue(pitch, PITCH_RATIO);

                    //yaw
                    joystickInput[3] = smoothValue(yaw, YAW_RATIO);

                    //chan5
                    if (btns[10])
                    {
                        joystickInput[4] = FlightModes["Stabilize"];
                    }
                    else if (btns[11])
                    {
                        joystickInput[4] = FlightModes["Loiter"];
                    }
                    else if (btns[8])
                    {
                        joystickInput[4] = FlightModes["AltHold"];
                    }
                    else if (btns[9])
                    {
                        joystickInput[4] = FlightModes["PosHold"];
                    }

                    args.Buttons = btns;
                    args.jInput = joystickInput;
                    OnJoystickData(args);
                    Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                this.Release();
                LogEventArgs arg = new LogEventArgs();
                arg.msg = e.Message;
                OnJoystickFailed(arg);
            }
        }

        private ushort smoothValue(int value, int ratio, bool reverse = false)
        {
            value = Convert.ToInt32((Math.Pow(value, 3)) / (Math.Pow(ratio, 2)) + PWM_MIDDLE);
            value = (reverse) ? PWM_MIDDLE * 2 - value : value;

            return Convert.ToUInt16(value);
        }

        protected virtual void OnJoystickData(JoystickDataEventArgs e)
        {
            EventHandler<JoystickDataEventArgs> handler = NewJoystickData;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnJoystickFailed(LogEventArgs e)
        {
            EventHandler<LogEventArgs> handler = JoystickFailed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// 
        /// Release joystick
        /// 
        public void Release()
        {
            try
            {
                this.jPolling = false;
                Joystick.Unacquire();
                Joystick.Dispose();
            }
            catch
            {
                //nop
            }
        }
    }

    public class JoystickDataEventArgs : EventArgs
    {
        public ushort[] jInput { get; set; }
        public bool[] Buttons { get; set; }
    }
}
