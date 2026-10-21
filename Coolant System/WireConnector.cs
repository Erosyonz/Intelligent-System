using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace Coolant_System
{
    [ToolboxItem(true)]
    public class WireConnector : Control
    {
        private Timer pulseTimer;
        private float dashOffset = 0f;

        public Control SourceControl { get; set; }
        public Control TargetControl { get; set; }

        public Color WireColor { get; set; } = Color.Gold;
        public float WireThickness { get; set; } = 2.0f;
        public bool IsActive { get; set; } = true;

        public WireConnector()
        {
            this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                           ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;

            pulseTimer = new Timer();
            pulseTimer.Interval = 20;
            pulseTimer.Tick += (s, e) =>
            {
                if (IsActive)
                {
                    dashOffset -= 1.0f;
                    if (dashOffset < -20f) dashOffset = 0f;
                    this.Invalidate();
                }
            };
            pulseTimer.Start();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (this.Parent != null)
            {
                // Ensure this overlay control covers the entire form client area
                this.Dock = DockStyle.Fill;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (SourceControl == null || TargetControl == null) return;

            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Compute exact positions directly relative to the Parent form
            Point throttlePoint = new Point(
                SourceControl.Left + (SourceControl.Width / 2),
                SourceControl.Top
            );

            Point enginePoint = new Point(
                TargetControl.Left + (TargetControl.Width / 2),
                TargetControl.Bottom
            );

            // 1. Dark Sheath Background (3px)
            using (Pen casingPen = new Pen(Color.FromArgb(40, 40, 40), WireThickness + 1.5f))
            {
                casingPen.StartCap = LineCap.Round;
                casingPen.EndCap = LineCap.Round;
                g.DrawLine(casingPen, throttlePoint, enginePoint);
            }

            // 2. Main Signal Wire Core
            using (Pen corePen = new Pen(WireColor, WireThickness))
            {
                corePen.StartCap = LineCap.Round;
                corePen.EndCap = LineCap.Round;
                g.DrawLine(corePen, throttlePoint, enginePoint);
            }

            // 3. Animated Signal Flow Pulse
            if (IsActive)
            {
                using (Pen pulsePen = new Pen(Color.White, 1.0f))
                {
                    pulsePen.DashStyle = DashStyle.Dash;
                    pulsePen.DashPattern = new float[] { 3f, 5f };
                    pulsePen.DashOffset = dashOffset;
                    g.DrawLine(pulsePen, throttlePoint, enginePoint);
                }
            }

            // 4. Pin Terminals
            using (SolidBrush terminalBrush = new SolidBrush(Color.Silver))
            {
                g.FillEllipse(terminalBrush, throttlePoint.X - 3, throttlePoint.Y - 3, 6, 6);
                g.FillEllipse(terminalBrush, enginePoint.X - 3, enginePoint.Y - 3, 6, 6);
                g.DrawEllipse(Pens.Black, throttlePoint.X - 3, throttlePoint.Y - 3, 6, 6);
                g.DrawEllipse(Pens.Black, enginePoint.X - 3, enginePoint.Y - 3, 6, 6);
            }
        }
    }
}