using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace Coolant_System
{
    public delegate void TemperatureChangedEventHandler(object sender, float currentTemp);

    [ToolboxItem(true)]
    public class Thermostat : Control
    {
        private Timer physicsTimer;

        // Dynamic System Parameters
        public float CurrentTemperature { get; set; } = 85.0f;
        public float OpeningTemperature { get; set; } = 90.0f;
        public float MaxTemperature { get; set; } = 120.0f;

        public bool IsValveOpen => CurrentTemperature >= OpeningTemperature;

        // Custom Events
        public event TemperatureChangedEventHandler TemperatureChanged;

        public Thermostat()
        {
            this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.Size = new Size(130, 100);

            physicsTimer = new Timer();
            physicsTimer.Interval = 100;
            physicsTimer.Tick += (s, e) => this.Invalidate();
            physicsTimer.Start();
        }

        public void SimulateThermalLoad(bool isEngineRunning, bool isFanOn)
        {
            if (isEngineRunning) CurrentTemperature += 0.4f;

            if (isFanOn && IsValveOpen)
            {
                CurrentTemperature -= 0.7f;
            }
            else if (!isEngineRunning && CurrentTemperature > 25.0f)
            {
                CurrentTemperature -= 0.2f;
            }

            CurrentTemperature = Math.Max(25.0f, Math.Min(MaxTemperature, CurrentTemperature));
            TemperatureChanged?.Invoke(this, CurrentTemperature);
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Width < 20 || this.Height < 20) return;

            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color darkPlastic = Color.FromArgb(35, 38, 42);
            Color plasticHighlight = Color.FromArgb(70, 75, 82);
            Color blueHousingColor = Color.FromArgb(38, 55, 95);
            Color brassColor = Color.FromArgb(218, 165, 32);

            // 1. Bottom Mounting Ears with Brass Bushings
            Rectangle bottomEar = new Rectangle(45, 62, 32, 28);
            using (SolidBrush earBrush = new SolidBrush(darkPlastic))
            using (Pen earPen = new Pen(plasticHighlight, 1.5f))
            {
                g.FillRectangle(earBrush, bottomEar);
                g.DrawRectangle(earPen, bottomEar);
            }

            // Brass Bushing Inner
            Rectangle brassOuter = new Rectangle(53, 68, 16, 16);
            Rectangle brassInner = new Rectangle(57, 72, 8, 8);
            using (SolidBrush bBrush = new SolidBrush(brassColor))
            using (SolidBrush holeBrush = new SolidBrush(Color.FromArgb(20, 20, 20)))
            {
                g.FillEllipse(bBrush, brassOuter);
                g.FillEllipse(holeBrush, brassInner);
            }

            // 2. Main Horizontal Black Housing Body
            Rectangle mainBody = new Rectangle(30, 28, 55, 36);
            using (LinearGradientBrush bodyBrush = new LinearGradientBrush(mainBody, plasticHighlight, darkPlastic, LinearGradientMode.Vertical))
            {
                g.FillRectangle(bodyBrush, mainBody);
            }

            // Molding Ridge Details ("PPA-GF35")
            using (Pen detailPen = new Pen(Color.FromArgb(20, 22, 25), 2))
            {
                g.DrawLine(detailPen, 35, 38, 70, 38);
                g.DrawLine(detailPen, 35, 48, 70, 48);
            }

            // 3. Left Lower Angled Pipe Nozzle
            GraphicsState stateLeft = g.Save();
            g.TranslateTransform(25, 50);
            g.RotateTransform(35);
            Rectangle leftNozzle = new Rectangle(-25, -10, 38, 20);
            using (LinearGradientBrush leftPipeBrush = new LinearGradientBrush(leftNozzle, plasticHighlight, darkPlastic, LinearGradientMode.Vertical))
            {
                g.FillRectangle(leftPipeBrush, leftNozzle);
                g.DrawEllipse(Pens.Black, -25, -10, 8, 20); // Nozzle lip
            }
            g.Restore(stateLeft);

            // 4. Blue Cap Collar Ring
            Rectangle blueCollar = new Rectangle(80, 24, 14, 44);
            using (LinearGradientBrush blueBrush = new LinearGradientBrush(blueCollar, Color.FromArgb(65, 90, 150), blueHousingColor, LinearGradientMode.Horizontal))
            {
                g.FillRectangle(blueBrush, blueCollar);
            }

            // 5. Right Upper Angled Pipe Nozzle (Blue Outlet)
            GraphicsState stateRight = g.Save();
            g.TranslateTransform(90, 35);
            g.RotateTransform(-35);
            Rectangle rightNozzle = new Rectangle(0, -9, 40, 18);
            using (LinearGradientBrush rightPipeBrush = new LinearGradientBrush(rightNozzle, Color.FromArgb(60, 85, 140), blueHousingColor, LinearGradientMode.Vertical))
            {
                g.FillRectangle(rightPipeBrush, rightNozzle);
                g.FillEllipse(Brushes.DarkBlue, 36, -9, 7, 18); // Nozzle opening
            }
            g.Restore(stateRight);

            // 6. Real-Time Overlay Badge (Valve Status + Temperature)
            Rectangle badgeRect = new Rectangle(5, 5, 120, 20);
            using (SolidBrush badgeBG = new SolidBrush(Color.FromArgb(180, 15, 15, 18)))
            {
                g.FillRectangle(badgeBG, badgeRect);
            }

            Color statusColor = IsValveOpen ? Color.LimeGreen : Color.Crimson;
            using (Pen badgeBorder = new Pen(statusColor, 1.5f))
            {
                g.DrawRectangle(badgeBorder, badgeRect);
            }

            using (Font textFont = new Font("Segoe UI", 8f, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            using (SolidBrush statusBrush = new SolidBrush(statusColor))
            {
                string tempStr = $"{CurrentTemperature:F1}°C";
                string stateStr = IsValveOpen ? "OPEN" : "CLOSED";

                g.DrawString(tempStr, textFont, textBrush, 8, 7);
                g.DrawString(stateStr, textFont, statusBrush, 82, 7);
            }
        }
    }
}