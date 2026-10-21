using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Coolant_System
{
    public class DrawingFan
    {
        public float CurrentAngle { get; set; } = 0;
        public float SpinSpeed { get; set; } = 0;
        public float TargetSpinSpeed { get; set; } = 15f;
        public bool IsPowered { get; set; } = false;

        private int bladeCount = 5;
        private Color bladeColor = Color.FromArgb(180, 70, 80, 95);
        private Color housingColor = Color.FromArgb(220, 40, 45, 50);

        public void UpdatePhysics()
        {
            CurrentAngle += SpinSpeed;

            if (IsPowered)
            {
                if (SpinSpeed < TargetSpinSpeed)
                    SpinSpeed += 0.5f;
                else if (SpinSpeed > TargetSpinSpeed)
                    SpinSpeed -= 0.5f;
            }
            else
            {
                if (SpinSpeed > 0) SpinSpeed *= 0.98f;
            }

            if (SpinSpeed < 0.1f) SpinSpeed = 0;
        }

        public void Draw(Graphics g, Rectangle bounds)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int size = Math.Min(bounds.Width, bounds.Height);
            int centerX = bounds.X + (bounds.Width / 2);
            int centerY = bounds.Y + (bounds.Height / 2);
            Rectangle rect = new Rectangle(centerX - (size / 2), centerY - (size / 2), size, size);

            // 1. Draw Outer Housing
            using (Pen grillPen = new Pen(housingColor, 4))
            {
                g.DrawEllipse(grillPen, rect);
            }

            // 2. Draw Fan Blades with Rotational Vector Transform
            float angleStep = 360f / bladeCount;
            for (int i = 0; i < bladeCount; i++)
            {
                GraphicsState state = g.Save();
                g.TranslateTransform(centerX, centerY);
                g.RotateTransform(CurrentAngle + (i * angleStep));

                using (GraphicsPath bladePath = new GraphicsPath())
                {
                    bladePath.AddBezier(new Point(0, -10), new Point(size / 6, -size / 3), new Point(size / 3, -size / 2), new Point(0, -size / 2));
                    bladePath.AddBezier(new Point(0, -size / 2), new Point(-size / 6, -size / 3), new Point(-size / 12, -10), new Point(0, -10));

                    using (SolidBrush brush = new SolidBrush(bladeColor)) g.FillPath(brush, bladePath);
                    using (Pen highlightPen = new Pen(Color.FromArgb(50, Color.White), 1)) g.DrawPath(highlightPen, bladePath);
                }
                g.Restore(state);
            }

            // 3. Draw Center Hub
            int hubSize = size / 4;
            Rectangle hubRect = new Rectangle(centerX - (hubSize / 2), centerY - (hubSize / 2), hubSize, hubSize);
            using (LinearGradientBrush hubBrush = new LinearGradientBrush(hubRect, Color.DarkGray, housingColor, LinearGradientMode.ForwardDiagonal))
            {
                g.FillEllipse(hubBrush, hubRect);
            }
            g.DrawEllipse(Pens.Black, hubRect);
        }
    } 
    public partial class coolingFan : UserControl
    {
        private DrawingFan fanEngine = new DrawingFan();

        public float TargetSpinSpeed
        {
            get => fanEngine.TargetSpinSpeed;
            set => fanEngine.TargetSpinSpeed = value;
        }

        public void TogglePower(bool power)
        {
            fanEngine.IsPowered = power;
        }

        public bool IsRunningContinuously()
        {
            return fanEngine.IsPowered;
        }

        private void fanTimer_Tick(object sender, EventArgs e)
        {
            fanEngine.UpdatePhysics();
            this.Invalidate();
        }
    }

    public class DrawingRadiator
    {
        public void Draw(Graphics g, Rectangle bounds)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 1. Core Background Fin Mesh
            using (SolidBrush finBackground = new SolidBrush(Color.FromArgb(45, 45, 50)))
            {
                g.FillRectangle(finBackground, bounds);
            }

            // 2. Core Tubes
            using (Pen tubePen = new Pen(Color.FromArgb(120, 125, 130), 3))
            {
                for (int y = bounds.Y + 6; y < bounds.Bottom; y += 8)
                {
                    g.DrawLine(tubePen, bounds.X, y, bounds.Right, y);
                }
            }

            // 3. Heavy Outer Tank Frame
            using (Pen framePen = new Pen(Color.FromArgb(70, 75, 80), 6))
            {
                g.DrawRectangle(framePen, bounds);
            }

            // 4. Input/Output Ports
            int portSize = 20;
            Rectangle hotPort = new Rectangle(bounds.X + 10, bounds.Y - (portSize / 2), portSize, portSize);
            Rectangle coldPort = new Rectangle(bounds.Right - 30, bounds.Bottom - (portSize / 2), portSize, portSize);

            g.FillEllipse(Brushes.Crimson, hotPort);
            g.DrawEllipse(Pens.Black, hotPort);

            g.FillEllipse(Brushes.RoyalBlue, coldPort);
            g.DrawEllipse(Pens.Black, coldPort);
        }
    }
}