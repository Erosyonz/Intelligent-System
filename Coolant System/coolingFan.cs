using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Coolant_System
{
    [ToolboxItem(true)]
    public class CoolingFan : Control
    {
        private DrawingFan fanEngine = new DrawingFan();
        private Timer animationTimer;

        public float TargetSpinSpeed { get; internal set; }

        public CoolingFan()
        {
            this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.Size = new Size(120, 120);

            animationTimer = new Timer();
            animationTimer.Interval = 20; // ~50 FPS
            animationTimer.Tick += (s, e) =>
            {
                fanEngine.UpdatePhysics();
                this.Invalidate();
            };
            animationTimer.Start();
        }

        public void TogglePower(bool powerOn)
        {
            fanEngine.IsPowered = powerOn;
        }

        public bool IsRunningContinuously()
        {
            return fanEngine.IsPowered || fanEngine.SpinSpeed > 1.0f;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            fanEngine.Draw(e.Graphics, this.ClientRectangle);
        }
    }
}