using System.Drawing;
using System.Windows.Forms;

namespace MavControl
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.throttleNum = new System.Windows.Forms.Label();
            this.Console = new System.Windows.Forms.RichTextBox();
            this.throttleBar = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.yawBar = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.pingLabel = new System.Windows.Forms.Label();
            this.AltLabel = new System.Windows.Forms.Label();
            this.AltValue = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.SpeedValue = new System.Windows.Forms.Label();
            this.SpeedLabel = new System.Windows.Forms.Label();
            this.HeadingValue = new System.Windows.Forms.Label();
            this.HeadingLabel = new System.Windows.Forms.Label();
            this.SysStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.throttleBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yawBar)).BeginInit();
            this.SuspendLayout();
            // 
            // throttleNum
            // 
            this.throttleNum.AutoSize = true;
            this.throttleNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.throttleNum.Location = new System.Drawing.Point(123, 39);
            this.throttleNum.Name = "throttleNum";
            this.throttleNum.Size = new System.Drawing.Size(0, 25);
            this.throttleNum.TabIndex = 0;
            // 
            // Console
            // 
            this.Console.BackColor = System.Drawing.Color.Black;
            this.Console.ForeColor = System.Drawing.Color.Lime;
            this.Console.HideSelection = false;
            this.Console.Location = new System.Drawing.Point(12, 401);
            this.Console.Name = "Console";
            this.Console.ReadOnly = true;
            this.Console.Size = new System.Drawing.Size(850, 193);
            this.Console.TabIndex = 1;
            this.Console.Text = "";
            // 
            // throttleBar
            // 
            this.throttleBar.Enabled = false;
            this.throttleBar.Location = new System.Drawing.Point(817, 12);
            this.throttleBar.Maximum = 2100;
            this.throttleBar.Minimum = 900;
            this.throttleBar.Name = "throttleBar";
            this.throttleBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.throttleBar.Size = new System.Drawing.Size(45, 327);
            this.throttleBar.TabIndex = 9;
            this.throttleBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.throttleBar.Value = 1000;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(778, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "2100";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(790, 322);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "900";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(784, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "1500";
            // 
            // yawBar
            // 
            this.yawBar.Enabled = false;
            this.yawBar.Location = new System.Drawing.Point(535, 294);
            this.yawBar.Maximum = 2100;
            this.yawBar.Minimum = 900;
            this.yawBar.Name = "yawBar";
            this.yawBar.Size = new System.Drawing.Size(200, 45);
            this.yawBar.TabIndex = 14;
            this.yawBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.yawBar.Value = 1500;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(630, 322);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(532, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "aux:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(559, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 13);
            this.label8.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(531, 361);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 20);
            this.label10.TabIndex = 20;
            this.label10.Text = "PING:";
            // 
            // pingLabel
            // 
            this.pingLabel.AutoSize = true;
            this.pingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pingLabel.Location = new System.Drawing.Point(594, 361);
            this.pingLabel.Name = "pingLabel";
            this.pingLabel.Size = new System.Drawing.Size(0, 20);
            this.pingLabel.TabIndex = 21;
            // 
            // AltLabel
            // 
            this.AltLabel.AutoSize = true;
            this.AltLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AltLabel.Location = new System.Drawing.Point(341, 31);
            this.AltLabel.Name = "AltLabel";
            this.AltLabel.Size = new System.Drawing.Size(46, 20);
            this.AltLabel.TabIndex = 23;
            this.AltLabel.Text = "ALT:";
            // 
            // AltValue
            // 
            this.AltValue.AutoSize = true;
            this.AltValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AltValue.Location = new System.Drawing.Point(396, 31);
            this.AltValue.Name = "AltValue";
            this.AltValue.Size = new System.Drawing.Size(0, 20);
            this.AltValue.TabIndex = 24;
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StatusLabel.Location = new System.Drawing.Point(21, 361);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 20);
            this.StatusLabel.TabIndex = 25;
            // 
            // SpeedValue
            // 
            this.SpeedValue.AutoSize = true;
            this.SpeedValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SpeedValue.Location = new System.Drawing.Point(396, 70);
            this.SpeedValue.Name = "SpeedValue";
            this.SpeedValue.Size = new System.Drawing.Size(0, 20);
            this.SpeedValue.TabIndex = 27;
            // 
            // SpeedLabel
            // 
            this.SpeedLabel.AutoSize = true;
            this.SpeedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SpeedLabel.Location = new System.Drawing.Point(341, 70);
            this.SpeedLabel.Name = "SpeedLabel";
            this.SpeedLabel.Size = new System.Drawing.Size(50, 20);
            this.SpeedLabel.TabIndex = 26;
            this.SpeedLabel.Text = "SPD:";
            // 
            // HeadingValue
            // 
            this.HeadingValue.AutoSize = true;
            this.HeadingValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.HeadingValue.Location = new System.Drawing.Point(396, 108);
            this.HeadingValue.Name = "HeadingValue";
            this.HeadingValue.Size = new System.Drawing.Size(0, 20);
            this.HeadingValue.TabIndex = 29;
            // 
            // HeadingLabel
            // 
            this.HeadingLabel.AutoSize = true;
            this.HeadingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.HeadingLabel.Location = new System.Drawing.Point(341, 108);
            this.HeadingLabel.Name = "HeadingLabel";
            this.HeadingLabel.Size = new System.Drawing.Size(54, 20);
            this.HeadingLabel.TabIndex = 28;
            this.HeadingLabel.Text = "HDG:";
            // 
            // ModeStatus
            // 
            this.SysStatus.AutoSize = true;
            this.SysStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SysStatus.Location = new System.Drawing.Point(21, 15);
            this.SysStatus.Name = "ModeStatus";
            this.SysStatus.Size = new System.Drawing.Size(0, 20);
            this.SysStatus.TabIndex = 30;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 606);
            this.Controls.Add(this.SysStatus);
            this.Controls.Add(this.HeadingValue);
            this.Controls.Add(this.HeadingLabel);
            this.Controls.Add(this.SpeedValue);
            this.Controls.Add(this.SpeedLabel);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.AltValue);
            this.Controls.Add(this.AltLabel);
            this.Controls.Add(this.pingLabel);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.yawBar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.throttleBar);
            this.Controls.Add(this.Console);
            this.Controls.Add(this.throttleNum);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.Text = "MAVLink Ground Control Experiment";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.throttleBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yawBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label throttleNum;
        private System.Windows.Forms.RichTextBox Console;
        private System.Windows.Forms.TrackBar throttleBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar yawBar;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label10;
        private Label pingLabel;
        private Label AltLabel;
        private Label AltValue;
        private Label StatusLabel;
        private Label SpeedValue;
        private Label SpeedLabel;
        private Label HeadingValue;
        private Label HeadingLabel;
        private Label SysStatus;
    }
}

