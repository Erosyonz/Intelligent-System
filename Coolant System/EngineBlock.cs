using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace Coolant_System
{
    [ToolboxItem(true)]
    public class EngineBlock : Control
    {
        private Timer animationTimer;
        public float pistonPhase = 0f;

        // Visual & Operational Properties
        public bool IsRunning { get; set; } = true;
        public float Temperature { get; set; } = 85.0f; // Heat drives the engine glow effect

        // --- NEW: Speed property to scale animation ---
        private float _speed = 1.0f;
        [Category("Engine Settings")]
        [DefaultValue(1.0f)]
        public float Speed
        {
            get => _speed;
            set
            {
                _speed = Math.Max(0f, value); // Ensure speed doesn't go negative
            }
        }

        public EngineBlock()
        {
            this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                           ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.Size = new Size(180, 130);

            // Piston movement loop (~60 FPS)
            animationTimer = new Timer();
            animationTimer.Interval = 16;
            animationTimer.Tick += (s, e) =>
            {
                if (IsRunning && Speed > 0)
                {
                    // Scale base step (0.15f) by Speed multiplier
                    pistonPhase += 0.15f * Speed;
                    if (pistonPhase > Math.PI * 2) pistonPhase %= (float)(Math.PI * 2);
                    this.Invalidate();
                }
            };
            animationTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Width < 30 || this.Height < 30) return;

            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int margin = 8;
            Rectangle mainBlock = new Rectangle(margin, margin, this.Width - (margin * 2), this.Height - (margin * 2));

            // 1. Draw Cast Iron Block Outer Frame
            using (LinearGradientBrush blockBrush = new LinearGradientBrush(mainBlock, Color.FromArgb(60, 65, 70), Color.FromArgb(30, 32, 35), LinearGradientMode.Vertical))
            {
                g.FillRectangle(blockBrush, mainBlock);
            }
            using (Pen framePen = new Pen(Color.FromArgb(90, 95, 100), 3))
            {
                g.DrawRectangle(framePen, mainBlock);
            }

            // 2. Heat Glow Overlay (Glows brighter crimson as temperature rises)
            int glowAlpha = (int)Math.Min(120, Math.Max(0, (Temperature - 60.0f) * 2.2f));
            if (glowAlpha > 0)
            {
                using (SolidBrush heatGlow = new SolidBrush(Color.FromArgb(glowAlpha, 220, 40, 20)))
                {
                    g.FillRectangle(heatGlow, mainBlock);
                }
            }

            // 3. Draw 4 Cylinders with Animated Pistons
            int cylinderCount = 4;
            int padding = 10;
            int availWidth = mainBlock.Width - (padding * (cylinderCount + 1));
            int cylWidth = availWidth / cylinderCount;
            int cylHeight = mainBlock.Height - 30;
            int cylY = mainBlock.Y + 15;

            for (int i = 0; i < cylinderCount; i++)
            {
                int cylX = mainBlock.X + padding + i * (cylWidth + padding);
                Rectangle cylRect = new Rectangle(cylX, cylY, cylWidth, cylHeight);

                // Cylinder Bore Wall
                using (SolidBrush boreBrush = new SolidBrush(Color.FromArgb(20, 22, 25)))
                {
                    g.FillRectangle(boreBrush, cylRect);
                }
                g.DrawRectangle(Pens.DimGray, cylRect);

                // Piston Reciprocating Offset (Alternating firing order)
                float offsetMultiplier = (i % 2 == 0) ? 1.0f : -1.0f;
                float stroke = (cylHeight - 24) / 2.0f;
                int pistonY = (int)(cylY + (cylHeight / 2) - 10 + Math.Sin(pistonPhase * offsetMultiplier) * stroke);

                // Piston Head
                Rectangle pistonRect = new Rectangle(cylX + 2, pistonY, cylWidth - 4, 18);
                using (LinearGradientBrush pistonBrush = new LinearGradientBrush(pistonRect, Color.Silver, Color.Gray, LinearGradientMode.Vertical))
                {
                    g.FillRectangle(pistonBrush, pistonRect);
                }
                g.DrawRectangle(Pens.Black, pistonRect);

                // Connecting Rod
                g.DrawLine(Pens.DarkGray, cylX + (cylWidth / 2), pistonRect.Bottom, cylX + (cylWidth / 2), cylRect.Bottom);
            }

            // 4. Header Label
            using (Font font = new Font("Segoe UI", 7f, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(200, Color.White)))
            {
                string label = IsRunning ? $"ENGINE: RUNNING ({Speed:F1}x)" : "ENGINE: OFF";
                g.DrawString(label, font, textBrush, mainBlock.X + 6, mainBlock.Y + 2);
            }
        }
    }
}