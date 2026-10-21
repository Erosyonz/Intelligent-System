using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Coolant_System
{
    [ToolboxItem(true)]
    public class Radiator : Control
    {
        public Radiator()
        {
            this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.Size = new Size(240, 160);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Width <= 30 || this.Height <= 30) return;

            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int frameThickness = 8;
            Rectangle coreRect = new Rectangle(frameThickness, 25, this.Width - (frameThickness * 2), this.Height - 50);

            // 1. Draw Background Cooling Fin Block
            using (SolidBrush finBackground = new SolidBrush(Color.FromArgb(45, 45, 50)))
            {
                g.FillRectangle(finBackground, coreRect);
            }

            // 2. Draw Horizontal Core Tubes & Fin Grids
            int lineSpacing = 8;
            using (Pen finPen = new Pen(Color.FromArgb(80, 85, 90), 1))
            using (Pen tubePen = new Pen(Color.FromArgb(120, 125, 130), 3))
            {
                for (int y = coreRect.Y + 4; y < coreRect.Bottom; y += lineSpacing)
                {
                    g.DrawLine(tubePen, coreRect.X, y, coreRect.Right, y);

                    for (int x = coreRect.X + 5; x < coreRect.Right; x += 12)
                    {
                        g.DrawLine(finPen, x, y - 2, x, y + 2);
                    }
                }
            }

            // 3. Draw Outer Heavy Industrial Frame
            using (Pen framePen = new Pen(Color.FromArgb(70, 75, 80), frameThickness))
            {
                g.DrawRectangle(framePen, coreRect);
            }

            // 4. Draw Coolant Ports
           

            // 5. Draw Labels
            
        }
    }
}