using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunoLau.ImageToGradient
{
    public class GradientResult
    {
        public double Angle { get; set; }
        public byte[][] Gradient { get; set; }

        public GradientResult(int size)
        {
            Gradient = new byte[size][];
        }

        public string BuildCssGradient()
        {
            var builder = new StringBuilder("linear-gradient(");
            if (Angle != 0)
            {
                builder.Append(Angle + "deg,");
            }

            for (var i = 0; i < Gradient.Length; i++)
            {
                var item = Gradient[i];
                var alpha = Math.Round(item[3] / 255.0, 2);
                builder.Append($"rgba({item[0]},{item[1]},{item[2]},{Math.Round(item[3] / 255.0, 2)}),");
            }

            builder.Remove(builder.Length - 1, 1);
            builder.Append(")");
            return builder.ToString();
        }

    }
}
