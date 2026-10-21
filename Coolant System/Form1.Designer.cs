namespace Coolant_System
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.coolantTube3 = new Coolant_System.CoolantTube();
            this.motorcycleThrottle1 = new Coolant_System.MotorcycleThrottle();
            this.coolantTube5 = new Coolant_System.CoolantTube();
            this.coolantTube4 = new Coolant_System.CoolantTube();
            this.coolantTube2 = new Coolant_System.CoolantTube();
            this.coolantTube1 = new Coolant_System.CoolantTube();
            this.hotCurveTube = new Coolant_System.CoolantTube();
            this.engineBlock1 = new Coolant_System.EngineBlock();
            this.thermostat1 = new Coolant_System.Thermostat();
            this.coolingFan1 = new Coolant_System.CoolingFan();
            this.hotInletTube = new Coolant_System.CoolantTube();
            this.coldCurveTube = new Coolant_System.CoolantTube();
            this.radiator1 = new Coolant_System.Radiator();
            this.coldOutletTube = new Coolant_System.CoolantTube();
            this.coolantTube6 = new Coolant_System.CoolantTube();
            this.SuspendLayout();
            // 
            // coolantTube3
            // 
            this.coolantTube3.BackColor = System.Drawing.Color.Transparent;
            this.coolantTube3.Direction = Coolant_System.FluidFlowDirection.Forward;
            this.coolantTube3.FluidColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.coolantTube3.Location = new System.Drawing.Point(406, 254);
            this.coolantTube3.Name = "coolantTube3";
            this.coolantTube3.Shape = Coolant_System.TubeShape.CurveTopToRight;
            this.coolantTube3.Size = new System.Drawing.Size(68, 76);
            this.coolantTube3.TabIndex = 11;
            this.coolantTube3.Text = "coolantTube3";
            this.coolantTube3.TubeThickness = 14;
            // 
            // motorcycleThrottle1
            // 
            this.motorcycleThrottle1.BackColor = System.Drawing.Color.Transparent;
            this.motorcycleThrottle1.Location = new System.Drawing.Point(576, 336);
            this.motorcycleThrottle1.Name = "motorcycleThrottle1";
            this.motorcycleThrottle1.Size = new System.Drawing.Size(160, 100);
            this.motorcycleThrottle1.TabIndex = 15;
            this.motorcycleThrottle1.Text = "motorcycleThrottle1";
            this.motorcycleThrottle1.ThrottlePosition = 0F;
            // 
            // coolantTube5
            // 
            this.coolantTube5.BackColor = System.Drawing.Color.Transparent;
            this.coolantTube5.Direction = Coolant_System.FluidFlowDirection.Forward;
            this.coolantTube5.FluidColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.coolantTube5.Location = new System.Drawing.Point(600, 164);
            this.coolantTube5.Name = "coolantTube5";
            this.coolantTube5.Shape = Coolant_System.TubeShape.CurveBottomToLeft;
            this.coolantTube5.Size = new System.Drawing.Size(107, 101);
            this.coolantTube5.TabIndex = 13;
            this.coolantTube5.Text = "coolantTube5";
            this.coolantTube5.TubeThickness = 14;
            // 
            // coolantTube4
            // 
            this.coolantTube4.BackColor = System.Drawing.Color.Transparent;
            this.coolantTube4.Direction = Coolant_System.FluidFlowDirection.Forward;
            this.coolantTube4.FluidColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.coolantTube4.Location = new System.Drawing.Point(464, 219);
            this.coolantTube4.Name = "coolantTube4";
            this.coolantTube4.Shape = Coolant_System.TubeShape.StraightHorizontal;
            this.coolantTube4.Size = new System.Drawing.Size(148, 76);
            this.coolantTube4.TabIndex = 12;
            this.coolantTube4.Text = "coolantTube4";
            this.coolantTube4.TubeThickness = 14;
            // 
            // coolantTube2
            // 
            this.coolantTube2.BackColor = System.Drawing.Color.Transparent;
            this.coolantTube2.Direction = Coolant_System.FluidFlowDirection.Forward;
            this.coolantTube2.FluidColor = System.Drawing.Color.Crimson;
            this.coolantTube2.Location = new System.Drawing.Point(339, 107);
            this.coolantTube2.Name = "coolantTube2";
            this.coolantTube2.Shape = Coolant_System.TubeShape.CurveTopToRight;
            this.coolantTube2.Size = new System.Drawing.Size(68, 76);
            this.coolantTube2.TabIndex = 10;
            this.coolantTube2.Text = "coolantTube2";
            this.coolantTube2.TubeThickness = 14;
            // 
            // coolantTube1
            // 
            this.coolantTube1.BackColor = System.Drawing.Color.Transparent;
            this.coolantTube1.Direction = Coolant_System.FluidFlowDirection.Forward;
            this.coolantTube1.FluidColor = System.Drawing.Color.Crimson;
            this.coolantTube1.Location = new System.Drawing.Point(287, 164);
            this.coolantTube1.Name = "coolantTube1";
            this.coolantTube1.Shape = Coolant_System.TubeShape.CurveBottomToLeft;
            this.coolantTube1.Size = new System.Drawing.Size(68, 76);
            this.coolantTube1.TabIndex = 9;
            this.coolantTube1.Text = "coolantTube1";
            this.coolantTube1.TubeThickness = 14;
            // 
            // hotCurveTube
            // 
            this.hotCurveTube.BackColor = System.Drawing.Color.Transparent;
            this.hotCurveTube.Direction = Coolant_System.FluidFlowDirection.Forward;
            this.hotCurveTube.FlowSpeed = 2F;
            this.hotCurveTube.FluidColor = System.Drawing.Color.Crimson;
            this.hotCurveTube.Location = new System.Drawing.Point(102, 228);
            this.hotCurveTube.Name = "hotCurveTube";
            this.hotCurveTube.Shape = Coolant_System.TubeShape.CurveTopToRight;
            this.hotCurveTube.Size = new System.Drawing.Size(80, 80);
            this.hotCurveTube.TabIndex = 3;
            this.hotCurveTube.TubeThickness = 14;
            // 
            // engineBlock1
            // 
            this.engineBlock1.BackColor = System.Drawing.Color.Transparent;
            this.engineBlock1.IsRunning = true;
            this.engineBlock1.Location = new System.Drawing.Point(586, 41);
            this.engineBlock1.Name = "engineBlock1";
            this.engineBlock1.Size = new System.Drawing.Size(180, 130);
            this.engineBlock1.TabIndex = 8;
            this.engineBlock1.Temperature = 85F;
            this.engineBlock1.Text = "engineBlock1";
            // 
            // thermostat1
            // 
            this.thermostat1.BackColor = System.Drawing.Color.Transparent;
            this.thermostat1.CurrentTemperature = 85F;
            this.thermostat1.Location = new System.Drawing.Point(397, 83);
            this.thermostat1.MaxTemperature = 120F;
            this.thermostat1.Name = "thermostat1";
            this.thermostat1.OpeningTemperature = 90F;
            this.thermostat1.Size = new System.Drawing.Size(130, 100);
            this.thermostat1.TabIndex = 7;
            this.thermostat1.Text = "thermostat1";
            // 
            // coolingFan1
            // 
            this.coolingFan1.BackColor = System.Drawing.Color.Transparent;
            this.coolingFan1.Location = new System.Drawing.Point(147, 324);
            this.coolingFan1.Name = "coolingFan1";
            this.coolingFan1.Size = new System.Drawing.Size(79, 83);
            this.coolingFan1.TabIndex = 6;
            this.coolingFan1.Text = "coolingFan1";
            // 
            // hotInletTube
            // 
            this.hotInletTube.BackColor = System.Drawing.Color.Transparent;
            this.hotInletTube.Direction = Coolant_System.FluidFlowDirection.Forward;
            this.hotInletTube.FlowSpeed = 2F;
            this.hotInletTube.FluidColor = System.Drawing.Color.Crimson;
            this.hotInletTube.Location = new System.Drawing.Point(173, 219);
            this.hotInletTube.Name = "hotInletTube";
            this.hotInletTube.Shape = Coolant_System.TubeShape.StraightHorizontal;
            this.hotInletTube.Size = new System.Drawing.Size(130, 30);
            this.hotInletTube.TabIndex = 2;
            this.hotInletTube.TubeThickness = 14;
            // 
            // coldCurveTube
            // 
            this.coldCurveTube.BackColor = System.Drawing.Color.Transparent;
            this.coldCurveTube.Direction = Coolant_System.FluidFlowDirection.Forward;
            this.coldCurveTube.FlowSpeed = 2F;
            this.coldCurveTube.FluidColor = System.Drawing.Color.RoyalBlue;
            this.coldCurveTube.Location = new System.Drawing.Point(339, 327);
            this.coldCurveTube.Name = "coldCurveTube";
            this.coldCurveTube.Shape = Coolant_System.TubeShape.CurveBottomToLeft;
            this.coldCurveTube.Size = new System.Drawing.Size(80, 80);
            this.coldCurveTube.TabIndex = 5;
            this.coldCurveTube.TubeThickness = 14;
            // 
            // radiator1
            // 
            this.radiator1.Location = new System.Drawing.Point(97, 289);
            this.radiator1.Name = "radiator1";
            this.radiator1.Size = new System.Drawing.Size(186, 147);
            this.radiator1.TabIndex = 0;
            // 
            // coldOutletTube
            // 
            this.coldOutletTube.BackColor = System.Drawing.Color.Transparent;
            this.coldOutletTube.Direction = Coolant_System.FluidFlowDirection.Forward;
            this.coldOutletTube.FlowSpeed = 2F;
            this.coldOutletTube.FluidColor = System.Drawing.Color.RoyalBlue;
            this.coldOutletTube.Location = new System.Drawing.Point(274, 385);
            this.coldOutletTube.Name = "coldOutletTube";
            this.coldOutletTube.Shape = Coolant_System.TubeShape.StraightHorizontal;
            this.coldOutletTube.Size = new System.Drawing.Size(81, 30);
            this.coldOutletTube.TabIndex = 4;
            this.coldOutletTube.TubeThickness = 14;
            // 
            // coolantTube6
            // 
            this.coolantTube6.BackColor = System.Drawing.Color.Transparent;
            this.coolantTube6.Direction = Coolant_System.FluidFlowDirection.Forward;
            this.coolantTube6.FluidColor = System.Drawing.Color.Crimson;
            this.coolantTube6.Location = new System.Drawing.Point(502, 57);
            this.coolantTube6.Name = "coolantTube6";
            this.coolantTube6.Shape = Coolant_System.TubeShape.StraightHorizontal;
            this.coolantTube6.Size = new System.Drawing.Size(100, 76);
            this.coolantTube6.TabIndex = 14;
            this.coolantTube6.Text = "coolantTube6";
            this.coolantTube6.TubeThickness = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.coolantTube3);
            this.Controls.Add(this.motorcycleThrottle1);
            this.Controls.Add(this.coolantTube5);
            this.Controls.Add(this.coolantTube4);
            this.Controls.Add(this.coolantTube2);
            this.Controls.Add(this.coolantTube1);
            this.Controls.Add(this.hotCurveTube);
            this.Controls.Add(this.engineBlock1);
            this.Controls.Add(this.thermostat1);
            this.Controls.Add(this.coolingFan1);
            this.Controls.Add(this.hotInletTube);
            this.Controls.Add(this.coldCurveTube);
            this.Controls.Add(this.radiator1);
            this.Controls.Add(this.coldOutletTube);
            this.Controls.Add(this.coolantTube6);
            this.Name = "Form1";
            this.Text = "Coolant System Simulation";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Radiator radiator1;
        private CoolantTube hotInletTube;
        private CoolantTube hotCurveTube;
        private CoolantTube coldOutletTube;
        private CoolantTube coldCurveTube;
        private Thermostat thermostat1;
        private CoolingFan coolingFan1;

        private EngineBlock engineBlock1;
        private CoolantTube coolantTube1;
        private CoolantTube coolantTube2;
        private CoolantTube coolantTube3;
        private CoolantTube coolantTube4;
        private CoolantTube coolantTube5;
        private CoolantTube coolantTube6;

        private MotorcycleThrottle motorcycleThrottle1;
    }
}