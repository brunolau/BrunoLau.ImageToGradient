using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunoLau.ImageToGradient
{
    internal class GradientWorkingDefinition
    {
        public GradientWorkingDefinition(int steps, double angle)
        {
            Steps = steps;
            Angle = angle;
            Red = new double[Steps];
            Blue = new double[Steps];
            Alpha = new double[Steps];
            Green = new double[Steps];
            Unit = new double[Steps];

            for (var i = 0; i < Steps; i++)
            {
                Red[i] = 0;
                Green[i] = 0;
                Blue[i] = 0;
                Alpha[i] = 0;
                Unit[i] = 0;
            }
        }

        public int Steps { get; private set; }
        public double Angle { get; private set; }
        public double[] Red { get; private set; }
        public double[] Green { get; private set; }
        public double[] Blue { get; private set; }
        public double[] Alpha { get; private set; }
        public double[] Unit { get; private set; }
    }
}
