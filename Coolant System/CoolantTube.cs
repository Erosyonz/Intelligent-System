using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace Coolant_System
{
    // Renamed to avoid clashing with System.Windows.Forms.FlowDirection
    public enum FluidFlowDirection
    {
        Forward,
        Reverse
    }

    public enum TubeShape
    {
        StraightHorizontal,
        StraightVertical,
        CurveTopToRight,   // ┌ Curve
        CurveTopToLeft,    // ┐ Curve
        CurveBottomToRight,// └ Curve
        CurveBottomToLeft  // ┘ Curve
    }

    [ToolboxItem(true)]
    public class CoolantTube : Control
    {
        private Timer animationTimer;
        private float dashOffset = 0;

        // --- ADJUSTABLE PROPERTIES ---
        public Color FluidColor { get; set; } = Color.Crimson;
        public TubeShape Shape { get; set; } = TubeShape.StraightHorizontal;
        public FluidFlowDirection Direction { get; set; } = FluidFlowDirection.Forward;
        public int TubeThickness { get; set; } = 14;

        [DefaultValue(1.5f)]
        public float FlowSpeed { get; set; } = 1.5f;

        public CoolantTube()
        {
            this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.Size = new Size(100, 100);

            animationTimer = new Timer();
            animationTimer.Interval = 30; // ~33 FPS
            animationTimer.Tick += (s, e) =>
            {
                if (Direction == FluidFlowDirection.Forward)
                    dashOffset -= FlowSpeed;
                else
                    dashOffset += FlowSpeed;

                this.Invalidate();
            };
            animationTimer.Start();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20; // WS_EX_TRANSPARENT: Layering support
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Width < 10 || this.Height < 10) return;

            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = GetTubePath())
            {
                // 1. Outer Pipe Shell
                using (Pen outerPen = new Pen(Color.FromArgb(180, 50, 55, 60), TubeThickness))
                {
                    outerPen.LineJoin = LineJoin.Round;
                    g.DrawPath(outerPen, path);
                }

                // 2. Dark Inner Channel
                using (Pen innerPen = new Pen(Color.FromArgb(40, 40, 45), TubeThickness - 4))
                {
                    innerPen.LineJoin = LineJoin.Round;
                    g.DrawPath(innerPen, path);
                }

                // 3. Animated Fluid (Dashed Line)
                if (FlowSpeed > 0)
                {
                    using (Pen fluidPen = new Pen(FluidColor, TubeThickness - 6))
                    {
                        fluidPen.LineJoin = LineJoin.Round;
                        fluidPen.DashStyle = DashStyle.Custom;
                        fluidPen.DashPattern = new float[] { 3f, 2f };
                        fluidPen.DashOffset = dashOffset;

                        g.DrawPath(fluidPen, path);
                    }
                }

                // 4. Highlight Reflection
                using (Pen highlightPen = new Pen(Color.FromArgb(60, Color.White), 2))
                {
                    highlightPen.LineJoin = LineJoin.Round;
                    g.DrawPath(highlightPen, path);
                }
            }
        }

        private GraphicsPath GetTubePath()
        {
            GraphicsPath path = new GraphicsPath();
            int margin = TubeThickness / 2 + 2;
            int w = this.Width - margin * 2;
            int h = this.Height - margin * 2;

            switch (Shape)
            {
                case TubeShape.StraightHorizontal:
                    path.AddLine(margin, this.Height / 2, this.Width - margin, this.Height / 2);
                    break;

                case TubeShape.StraightVertical:
                    path.AddLine(this.Width / 2, margin, this.Width / 2, this.Height - margin);
                    break;

                case TubeShape.CurveTopToRight:
                    path.AddArc(margin, margin, w * 2, h * 2, 180, 90);
                    break;

                case TubeShape.CurveTopToLeft:
                    path.AddArc(margin - w, margin, w * 2, h * 2, 270, 90);
                    break;

                case TubeShape.CurveBottomToRight:
                    path.AddArc(margin, margin - h, w * 2, h * 2, 90, 90);
                    break;

                case TubeShape.CurveBottomToLeft:
                    path.AddArc(margin - w, margin - h, w * 2, h * 2, 0, 90);
                    break;
            }

            return path;
        }
    }
}