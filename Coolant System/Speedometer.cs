using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Coolant_System
{
    [ToolboxItem(true)]
    public class Speedometer : Control
    {
        private float currentSpeed = 0f;
        public float CurrentSpeed
        {
            get => currentSpeed;
            set
            {
                currentSpeed = Math.Max(0f, Math.Min(maxSpeed, value));
                this.Invalidate(); // Trigger redraw on change
            }
        }

        private float maxSpeed = 140f;
        public float MaxSpeed
        {
            get => maxSpeed;
            set { maxSpeed = value; this.Invalidate(); }
        }

        public Speedometer()
        {
            this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                           ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.Size = new Size(160, 160);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Width < 30 || this.Height < 30) return;
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int size = Math.Min(this.Width, this.Height) - 12;
            int centerX = this.Width / 2;
            int centerY = this.Height / 2;
            Rectangle rect = new Rectangle(centerX - size / 2, centerY - size / 2, size, size);

            // 1. Draw Gauge Dial Background
            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(30, 32, 36)))
            {
                g.FillEllipse(bgBrush, rect);
            }
            using (Pen rimPen = new Pen(Color.FromArgb(70, 75, 80), 4))
            {
                g.DrawEllipse(rimPen, rect);
            }

            // Gauge Angle Definitions: 135 degrees (bottom-left) to 405 degrees (bottom-right)
            float startAngle = 135f;
            float sweepAngle = 270f;

            // 2. Draw Speed Ticks & Numbers
            int majorTicks = 7; // Ticks for 0, 20, 40, 60, 80, 100, 120, 140
            for (int i = 0; i <= majorTicks; i++)
            {
                float tickVal = (maxSpeed / majorTicks) * i;
                float angle = startAngle + (i * (sweepAngle / majorTicks));
                float rad = angle * (float)Math.PI / 180f;

                int outerR = (size / 2) - 8;
                int innerR = (size / 2) - 18;

                int x1 = centerX + (int)(outerR * Math.Cos(rad));
                int y1 = centerY + (int)(outerR * Math.Sin(rad));
                int x2 = centerX + (int)(innerR * Math.Cos(rad));
                int y2 = centerY + (int)(innerR * Math.Sin(rad));

                using (Pen tickPen = new Pen(Color.Silver, 2))
                {
                    g.DrawLine(tickPen, x1, y1, x2, y2);
                }
            }

            // 3. Draw Needle
            float speedRatio = currentSpeed / maxSpeed;
            float needleAngle = startAngle + (speedRatio * sweepAngle);
            float needleRad = needleAngle * (float)Math.PI / 180f;

            int needleLength = (size / 2) - 22;
            int needleX = centerX + (int)(needleLength * Math.Cos(needleRad));
            int needleY = centerY + (int)(needleLength * Math.Sin(needleRad));

            using (Pen needlePen = new Pen(Color.Crimson, 3))
            {
                g.DrawLine(needlePen, centerX, centerY, needleX, needleY);
            }

            // Center Cap
            int hubSize = 16;
            Rectangle hubRect = new Rectangle(centerX - hubSize / 2, centerY - hubSize / 2, hubSize, hubSize);
            g.FillEllipse(Brushes.DarkGray, hubRect);
            g.DrawEllipse(Pens.Black, hubRect);

            // 4. Digital Speed Readout
            using (Font font = new Font("Segoe UI", 9f, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(Color.Orange))
            {
                string text = $"{Math.Round(currentSpeed)} km/h";
                SizeF textSize = g.MeasureString(text, font);
                g.DrawString(text, font, textBrush, centerX - (textSize.Width / 2), centerY + (size / 5));
            }
        }
    }
}