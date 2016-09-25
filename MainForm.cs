using MavLinkNet;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace MavControl
{
    public partial class MainForm : Form
    {
        // Declare components
        private MavControl mc;
        private RCJoystick rc;

        // Declare Graphics
        private Graphics gfx;
        private SolidBrush br;

        // Load images
        private Bitmap HorizonBitmap = new Bitmap(Properties.Resources.horizon);
        private Bitmap BezelBitmap = new Bitmap(Properties.Resources.bezel);
        private Bitmap HeadingBitmap = new Bitmap(Properties.Resources.heading);
        private Bitmap WingsBitmap = new Bitmap(Properties.Resources.wings);
        private Bitmap JoystickGridBitmap = new Bitmap(Properties.Resources.JoystickGrid);

        // Draw Regions
        private Region HorizonRegion;
        private Region JoystickRegion;

        // Artificial Horizon Points
        private Point ptBoule = new Point(-13, -370); //Ground-Sky initial location
        private Point ptHeading = new Point(-580, 190); // Heading ticks
        private Point ptRotation = new Point(138, 190); // Point of rotation

        // Init angles
        private double PitchAngle = 0;
        private double RollAngle = 0;
        private double YawAngle = 0;
        private int jInputRoll = 0;
        private int jInputPitch = 0;

        // Init HUD data
        private decimal Altitude = 0;
        private decimal Speed = 0;
        private decimal Heading = 0;

        // Init Jpystick Display Data
        private int jInputThrottle = 0;
        private int jInputYaw = 0;
        private int jInputAux = 0;

        // Init Status texts
        private string SystemStatus = "";
        private string SystemMessage = "";
        private string ConsoleLogMessage = "";

        private bool LogMessageFlag;

        public MainForm()
        {
            InitializeComponent();

            //Drawings
            br = new SolidBrush(Color.Red);
            HorizonRegion = new Region(new Rectangle(12, 40, 300, 300));
            JoystickRegion = new Region(new Rectangle(535, 39, 210, 210));

            //Smoothing and Flickering
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            //Init main modules
            tryInitModules();
        }

        private void tryInitModules()
        {
            try
            {
                //Init Joystick
                rc = new RCJoystick();
                rc.JoystickFailed += WriteLog;
                WriteLog("Joystick initiated successfully.");

                //Init Mavcontrol
                mc = new MavControl();
                WriteLog("MAVControl initiated successfully.");

                //Add EventListeners
                rc.NewJoystickData += mc.sendJoysticInput;
                rc.NewJoystickData += new EventHandler<JoystickDataEventArgs>(OnJoystickInput);

                mc.TwoWayConnection += WriteLog;
                mc.UpdateAttitude += UpdateAttitude;
                mc.UpdateHUD += UpdateHUD;
                mc.UpdateStatus += UpdateSystemMessage;
                mc.HeartBeat += UpdateSystemStatus;

            }
            catch (Exception e)
            {
                MessageBoxButtons btns = MessageBoxButtons.OK;
                MessageBox.Show(e.Message, "Fatal ERROR", btns);
                this.Close();
            }
        }

        protected override void OnPaint(PaintEventArgs paintEvnt)
        {
            // Calling the base class OnPaint
            base.OnPaint(paintEvnt);

            // Clipping mask for Attitude Indicator
            paintEvnt.Graphics.Clip = HorizonRegion;
            paintEvnt.Graphics.FillRegion(Brushes.Black, paintEvnt.Graphics.Clip);

            // Make sure lines are drawn smoothly
            paintEvnt.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            // Create the graphics object
            gfx = paintEvnt.Graphics;

            // Adjust and draw horizon image
            RotateAndTranslateRollnPitch(paintEvnt, HorizonBitmap, -RollAngle, 0, ptBoule, -PitchAngle, ptRotation, 1);
            RotateAndTranslateYaw(paintEvnt, HeadingBitmap, YawAngle, -RollAngle, 0, ptHeading, -PitchAngle, ptRotation, 1);


            gfx.DrawImage(BezelBitmap, 12, 40); // Draw bezel image
            gfx.DrawImage(WingsBitmap, 87, 165); // Draw wings image


            //Draw Joystick Roll and Pitch Input
            gfx.Clip = JoystickRegion;
            gfx.DrawImage(JoystickGridBitmap, 535, 39);
            gfx.FillRectangle(this.br, 535 + Convert.ToInt32((jInputRoll - 1000) / 5) + 4, 39 + Convert.ToInt32((jInputPitch - 1000) / 5) + 4, 2, 2);

            //Update HUD Data
            this.AltValue.Text = Altitude.ToString() + " m";
            this.SpeedValue.Text = Speed.ToString() + " km/h";
            this.HeadingValue.Text = Heading.ToString() + " deg";
            this.throttleBar.Value = jInputThrottle;
            this.yawBar.Value = jInputYaw;
            this.label8.Text = rc.ModeFlights[jInputAux];

            //Update Messages
            this.SysStatus.Text = this.SystemStatus;
            this.StatusLabel.Text = this.SystemMessage;

            if (LogMessageFlag)
            {
                WriteLog(ConsoleLogMessage);
                LogMessageFlag = false;
            }
        }

        protected void RotateAndTranslateRollnPitch(PaintEventArgs pe, Image img, Double alphaRot, Double alphaTrs, Point ptImg, Double deltaPx, Point ptRot, float scaleFactor)
        {
            double beta = 0;
            double d = 0;
            float deltaXRot = 0;
            float deltaYRot = 0;
            float deltaXTrs = 0;
            float deltaYTrs = 0;

            // Rotation

            if (ptImg != ptRot)
            {
                // Internals coeffs
                if (ptRot.X != 0)
                {
                    beta = Math.Atan((double)ptRot.Y / (double)ptRot.X);
                }

                d = Math.Sqrt((ptRot.X * ptRot.X) + (ptRot.Y * ptRot.Y));

                // Computed offset
                deltaXRot = (float)(d * (Math.Cos(alphaRot - beta) - Math.Cos(alphaRot) * Math.Cos(alphaRot + beta) - Math.Sin(alphaRot) * Math.Sin(alphaRot + beta)));
                deltaYRot = (float)(d * (Math.Sin(beta - alphaRot) + Math.Sin(alphaRot) * Math.Cos(alphaRot + beta) - Math.Cos(alphaRot) * Math.Sin(alphaRot + beta)));
            }

            // Translation

            // Computed offset
            deltaXTrs = (float)(deltaPx * (Math.Sin(alphaTrs)));
            deltaYTrs = (float)(-deltaPx * (-Math.Cos(alphaTrs)));

            // Rotate image support
            pe.Graphics.RotateTransform((float)(alphaRot * 180 / Math.PI));

            // Dispay image
            pe.Graphics.DrawImage(img, (ptImg.X + deltaXRot + deltaXTrs) * scaleFactor, (ptImg.Y + deltaYRot + deltaYTrs) * scaleFactor, img.Width * scaleFactor, img.Height * scaleFactor);

            // Put image support as found
            pe.Graphics.RotateTransform((float)(-alphaRot * 180 / Math.PI));
        }

        protected void RotateAndTranslateYaw(PaintEventArgs pe, Image img, Double yawRot, Double alphaRot, Double alphaTrs, Point ptImg, Double deltaPx, Point ptRot, float scaleFactor)
        {
            double beta = 0;
            double d = 0;
            float deltaXRot = 0;
            float deltaYRot = 0;
            float deltaXTrs = 0;
            float deltaYTrs = 0;

            // Rotation

            if (ptImg != ptRot)
            {
                // Internals coeffs
                if (ptRot.X != 0)
                {
                    beta = Math.Atan((double)ptRot.Y / (double)ptRot.X);
                }

                d = Math.Sqrt((ptRot.X * ptRot.X) + (ptRot.Y * ptRot.Y));

                // Computed offset
                deltaXRot = (float)(d * (Math.Cos(alphaRot - beta) - Math.Cos(alphaRot) * Math.Cos(alphaRot + beta) - Math.Sin(alphaRot) * Math.Sin(alphaRot + beta) + yawRot));
                deltaYRot = (float)(d * (Math.Sin(beta - alphaRot) + Math.Sin(alphaRot) * Math.Cos(alphaRot + beta) - Math.Cos(alphaRot) * Math.Sin(alphaRot + beta)));
            }

            // Translation

            // Computed offset
            deltaXTrs = (float)(deltaPx * (Math.Sin(alphaTrs)));
            deltaYTrs = (float)(-deltaPx * (-Math.Cos(alphaTrs)));

            // Rotate image support
            pe.Graphics.RotateTransform((float)(alphaRot * 180 / Math.PI));

            // Dispay image
            pe.Graphics.DrawImage(img, (ptImg.X + deltaXRot + deltaXTrs) * scaleFactor, (ptImg.Y + deltaYRot + deltaYTrs) * scaleFactor, img.Width * scaleFactor, img.Height * scaleFactor);

            // Put image support as found
            pe.Graphics.RotateTransform((float)(-alphaRot * 180 / Math.PI));
        }

        private void UpdateAttitude(object sender, UasAttitude message)
        {
            this.RollAngle = (double)message.Roll;
            this.PitchAngle = (double)message.Pitch;
            this.YawAngle = (double)message.Yaw;

            //Invalidate(); // not needed because of OnJoystick frequent call
        }

        private void UpdateHUD(object sender, UasVfrHud message)
        {
            this.Altitude = Math.Round((decimal)(message.Alt / 100f), 2);
            this.Speed = Math.Round((decimal)(message.Groundspeed * 3.6f), 2);
            this.Heading = message.Heading;
        }

        private void UpdateSystemMessage(object sender, UasStatustext message)
        {
            this.SystemMessage = new String(message.Text);
        }

        private void WriteLog(object sender, LogEventArgs e)
        {
            this.ConsoleLogMessage = e.msg;
            this.LogMessageFlag = true;
        }


        private void UpdateSystemStatus(object sender, UasHeartbeat message)
        {
            this.SystemStatus = message.SystemStatus.ToString();
        }


        private void OnJoystickInput(object sender, JoystickDataEventArgs e)
        {
            this.jInputThrottle = e.jInput[0];
            this.jInputYaw = e.jInput[3];
            this.jInputAux = e.jInput[4];
            this.jInputRoll = e.jInput[1];
            this.jInputPitch = e.jInput[2];

            Invalidate();
        }

        private void WriteLog(string txt)
        {
            this.Console.AppendText("[ " + DateTime.Now.ToString("HH:mm:ss") + " ] #> " + txt + "\n");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                br.Dispose();
                gfx.Dispose();

                mc.Dispose();
                rc.Release();

                HorizonRegion.Dispose();
                JoystickRegion.Dispose();

                BezelBitmap.Dispose();
                HeadingBitmap.Dispose();
                WingsBitmap.Dispose();
                HorizonBitmap.Dispose();
                JoystickGridBitmap.Dispose();
            }
            catch
            {
                //nop
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            BezelBitmap.MakeTransparent(Color.Yellow); // Sets image transparency
            HeadingBitmap.MakeTransparent(Color.Black); // Sets image transparency
            WingsBitmap.MakeTransparent(Color.Yellow); // Sets image transparency
        }

    }

    public class LogEventArgs : EventArgs
    {
        public string msg { get; set; }
    }
}
