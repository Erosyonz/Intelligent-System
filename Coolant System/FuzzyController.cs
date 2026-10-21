using System;

namespace Coolant_System
{
    public class MamdaniFuzzyEngine
    {
        public double CalculatePumpSpeed(double temperature, double motorcycleSpeed)
        {
            // 1. FUZZIFICATION
            double tempLow = TriangularMembership(temperature, 70, 80, 90);
            double tempMed = TriangularMembership(temperature, 85, 95, 105);
            double tempHigh = TriangularMembership(temperature, 100, 115, 130);

            double speedLow = TriangularMembership(motorcycleSpeed, 0, 15, 30);
            double speedHigh = TriangularMembership(motorcycleSpeed, 40, 60, 100);

            // 2. RULE EVALUATION
            double rule1_strength = Math.Max(tempHigh, speedHigh); // Fast Pump
            double rule2_strength = Math.Min(tempMed, speedLow);   // Medium Pump
            double rule3_strength = tempLow;                      // Slow Pump

            // 3. IMPLICATION, AGGREGATION & DEFUZZIFICATION (Centroid)
            double sumNumerator = 0.0;
            double sumDenominator = 0.0;
            double step = 0.5;

            for (double y = 0.0; y <= 100.0; y += step)
            {
                double outSlow = TriangularMembership(y, 0.0, 0.0, 50.0);
                double outMed = TriangularMembership(y, 20.0, 50.0, 80.0);
                double outFast = TriangularMembership(y, 50.0, 100.0, 100.0);

                double clippedSlow = Math.Min(rule3_strength, outSlow);
                double clippedMed = Math.Min(rule2_strength, outMed);
                double clippedFast = Math.Min(rule1_strength, outFast);

                double aggregatedY = Math.Max(clippedSlow, Math.Max(clippedMed, clippedFast));

                sumNumerator += y * aggregatedY * step;
                sumDenominator += aggregatedY * step;
            }

            return (sumDenominator > 0.0) ? (sumNumerator / sumDenominator) : 0.0;
        }

        private double TriangularMembership(double x, double a, double b, double c)
        {
            if (x <= a || x >= c) 
                return 0.0;
            if (x == b) 
                return 1.0;
            if (x > a && x < b) 
                return (x - a) / (b - a);
            return (c - x) / (c - b);
        }
    }
}