using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace Coolant_System
{
    [ToolboxItem(true)]
    public class MotorcycleThrottle : Control
    {
        private Timer holdTimer;
        private bool isMouseHeld = false;

        private float throttlePosition = 0.0f;
        public float ThrottlePosition
        {
            get => throttlePosition;
            set
            {
                throttlePosition = Math.Max(0.0f, Math.Min(1.0f, value));
                ThrottleChanged?.Invoke(this, throttlePosition);
                this.Invalidate();
            }
        }

        public event EventHandler<float> ThrottleChanged;

        public MotorcycleThrottle()
        {
            this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                           ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.Size = new Size(160, 100);

            // Hold & Snap-Return Physics Loop (~60 FPS)
            holdTimer = new Timer();
            holdTimer.Interval = 16;
            holdTimer.Tick += HoldTimer_Tick;
            holdTimer.Start();
        }

        private void HoldTimer_Tick(object sender, EventArgs e)
        {
            if (isMouseHeld)
            {
                // Smoothly ramp UP throttle while holding click
                if (ThrottlePosition < 1.0f)
                {
                    ThrottlePosition += 0.03f;
                }
            }
            else
            {
                // Snap RETURN back to idle (0%) when released like a mechanical return spring
                if (ThrottlePosition > 0.0f)
                {
                    ThrottlePosition -= 0.05f;
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                isMouseHeld = true;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                isMouseHeld = false;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            // Auto-release throttle if mouse moves off the control while holding
            isMouseHeld = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Width < 30 || this.Height < 30) return;

            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Handlebar Base
            Rectangle barRect = new Rectangle(5, 40, this.Width - 10, 20);
            using (LinearGradientBrush barBrush = new LinearGradientBrush(barRect, Color.Silver, Color.DimGray, LinearGradientMode.Vertical))
            {
                g.FillRectangle(barBrush, barRect);
            }

            // Metal Clamshell Housing
            Rectangle housingRect = new Rectangle(15, 20, 35, 60);
            using (LinearGradientBrush housingBrush = new LinearGradientBrush(housingRect, Color.FromArgb(60, 65, 70), Color.FromArgb(25, 28, 30), LinearGradientMode.ForwardDiagonal))
            {
                g.FillRectangle(housingBrush, housingRect);
            }
            g.DrawRectangle(Pens.Black, housingRect);

            // Cable Housing
            using (Pen cablePen = new Pen(Color.FromArgb(20, 20, 20), 4))
            {
                g.DrawBezier(cablePen, new Point(32, 20), new Point(32, 5), new Point(50, 2), new Point(70, 2));
            }

            // Twist Grip Body
            int gripX = 50;
            int gripWidth = this.Width - 60;
            Rectangle gripRect = new Rectangle(gripX, 32, gripWidth, 36);

            using (LinearGradientBrush gripBrush = new LinearGradientBrush(gripRect, Color.FromArgb(45, 45, 45), Color.FromArgb(15, 15, 15), LinearGradientMode.Vertical))
            {
                g.FillRectangle(gripBrush, gripRect);
            }

            // Rubber Ribs
            using (Pen ribPen = new Pen(Color.FromArgb(25, 25, 25), 2))
            {
                for (int x = gripX + 8; x < gripX + gripWidth - 5; x += 6)
                {
                    g.DrawLine(ribPen, x, 32, x, 68);
                }
            }

            // Flange Ring
            Rectangle flangeRect = new Rectangle(gripX - 4, 28, 8, 44);
            using (SolidBrush flangeBrush = new SolidBrush(Color.FromArgb(20, 20, 20)))
            {
                g.FillEllipse(flangeBrush, flangeRect);
            }

            // Dynamic Indicator Line
            float currentOffset = throttlePosition * 18f;
            using (Pen indicatorPen = new Pen(isMouseHeld ? Color.OrangeRed : Color.Gold, 2))
            {
                g.DrawLine(indicatorPen, gripX + 12, 34 + currentOffset, gripX + gripWidth - 8, 34 + currentOffset);
            }

            // Status Badge
            using (Font font = new Font("Segoe UI", 7.5f, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            {
                string text = isMouseHeld ? $"REV: {Math.Round(ThrottlePosition * 100)}%" : $"IDLE: {Math.Round(ThrottlePosition * 100)}%";
                g.DrawString(text, font, textBrush, 15, 78);
            }
        }
    }
}