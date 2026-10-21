using System;
using System.Windows.Forms;

namespace Coolant_System
{
    public partial class Form1 : Form
    {
        private Timer engineSimulationTimer;

        // 1. Declare Speedometer control
        private Speedometer speedometer1;

        private MamdaniFuzzyEngine fuzzyEngine = new MamdaniFuzzyEngine();
        private double motorcycleSpeed = 0.0;

        public Form1()
        {
            InitializeComponent();

            // 2. Initialize and place Speedometer on the form
            speedometer1 = new Speedometer
            {
                Location = new System.Drawing.Point(20, 20), // Adjust X, Y position as needed
                Size = new System.Drawing.Size(160, 160)
            };
            this.Controls.Add(speedometer1);

            engineSimulationTimer = new Timer();
            engineSimulationTimer.Interval = 100;
            engineSimulationTimer.Tick += EngineSimulationTimer_Tick;
            engineSimulationTimer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void EngineSimulationTimer_Tick(object sender, EventArgs e)
        {
            if (thermostat1 == null || coolingFan1 == null || engineBlock1 == null || motorcycleThrottle1 == null) return;

            // 1. Read throttle input and simulate vehicle speed physics
            float throttleFactor = motorcycleThrottle1.ThrottlePosition;

            if (throttleFactor > 0.05f)
                motorcycleSpeed = Math.Min(140.0, motorcycleSpeed + (throttleFactor * 4.0));
            else
                motorcycleSpeed = Math.Max(0.0, motorcycleSpeed - 2.0);

            // Update Speedometer UI Control
            if (speedometer1 != null)
            {
                speedometer1.CurrentSpeed = (float)motorcycleSpeed;
            }

            // 2. Heat Generation based on throttle and engine load
            bool isFanActive = coolingFan1.IsRunningContinuously();
            thermostat1.SimulateThermalLoad(isEngineRunning: true, isFanActive);

            if (throttleFactor > 0.05f)
            {
                thermostat1.CurrentTemperature += throttleFactor * 0.6f;
            }

            // 3. FUZZY LOGIC INTEGRATION: Evaluate crisp outputs using Mamdani Engine
            double currentTemp = thermostat1.CurrentTemperature;
            double pumpSpeedPercent = fuzzyEngine.CalculatePumpSpeed(currentTemp, motorcycleSpeed); // 0.0% to 100.0%

            // 4. Update UI Components: Blend Fuzzy Pump Output AND Motorcycle Speed
            float pumpFactor = (float)(pumpSpeedPercent / 100.0); // Normalized 0.0 to 1.0
            float speedFactor = (float)(motorcycleSpeed / 140.0);   // Normalized 0.0 to 1.0

            // Combine pump work and vehicle speed to boost tube fluid movement (0.5 idle up to ~9.0 max speed)
            float totalFlowSpeed = 0.5f * (pumpFactor * 2.5f) + (speedFactor * 2.0f);
            SetTubeFlowSpeed(totalFlowSpeed);

            // Scale fan spin speed from both pump demand and motorcycle speed (ram air effect)
            float dynamicFanSpeed = 5.0f + (pumpFactor * 20.0f) + (speedFactor * 15.0f);
            coolingFan1.TargetSpinSpeed = dynamicFanSpeed;

            // Activate fan if pump speed is high, valve is open, or bike is moving fast
            bool shouldRunFan = pumpSpeedPercent > 40.0 || thermostat1.IsValveOpen || motorcycleSpeed > 20.0;
            coolingFan1.TogglePower(shouldRunFan);

            // 5. Dynamic Heat Extraction: High pump speed actively lowers engine temp
            double activeCooling = (pumpSpeedPercent / 100.0) * 0.45;
            thermostat1.CurrentTemperature = (float)Math.Max(70.0, thermostat1.CurrentTemperature - activeCooling);

            // Mirror thermostat temperature directly onto engine block glow effect
            engineBlock1.Temperature = thermostat1.CurrentTemperature;

            // Update Engine Piston Speed dynamically based on motorcycle speed
            float engineAnimationSpeed = (float)(motorcycleSpeed / 140.0) * 2.5f;
            engineBlock1.Speed = (motorcycleSpeed == 0) ? 0.3f : engineAnimationSpeed;
        }

        private void SetTubeFlowSpeed(float speed)
        {
            // Standard Loop Tubes
            hotInletTube.FlowSpeed = speed;
            hotCurveTube.FlowSpeed = speed;
            coldOutletTube.FlowSpeed = speed;
            coldCurveTube.FlowSpeed = speed;

            // Engine Circuit Tubes (coolantTube1 through coolantTube6)
            coolantTube1.FlowSpeed = speed;
            coolantTube2.FlowSpeed = speed;
            coolantTube3.FlowSpeed = speed;
            coolantTube4.FlowSpeed = speed;
            coolantTube5.FlowSpeed = speed;
            coolantTube6.FlowSpeed = speed;
        }
    }
}