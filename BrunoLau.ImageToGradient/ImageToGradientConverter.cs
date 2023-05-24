
namespace BrunoLau.ImageToGradient
{
    public class ImageToGradientConverter
    {
        private static HttpClient? httpClient;

        /// <summary>
        /// Gradient direction angle
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// How many steps should the gradient have
        /// </summary>
        public int Steps { get; set; }

        /// <summary>
        /// Process local image file into gradient
        /// </summary>
        /// <param name="filePath">Local file path to the image</param>
        /// <returns></returns>
        public GradientResult ProcessFile(string filePath)
        {
            return ProcessImage(Image.Load<Rgba32>(filePath));
        }

        /// <summary>
        /// Process image hosted on remote URL
        /// </summary>
        /// <param name="url">Image URL</param>
        /// <returns></returns>
        public async Task<GradientResult> ProcessUrlAsync(string url)
        {
            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }

            GradientResult retVal;
            using (var response = await httpClient.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();
                retVal = await ProcessStreamAsync(await response.Content.ReadAsStreamAsync());
            }

            return retVal;
        }

        /// <summary>
        /// Process image from stream
        /// </summary>
        /// <param name="stream">Image stream</param>
        /// <returns></returns>
        public async Task<GradientResult> ProcessStreamAsync(Stream stream)
        {
            return ProcessImage(await Image.LoadAsync<Rgba32>(stream));
        }

        public GradientResult ProcessImage(Image<Rgba32> img)
        {
            var gradient = GetGradientDefinition(img);
            NormalizeGradient(gradient);

            return BuildResult(gradient);
        }

        private GradientResult BuildResult(GradientWorkingDefinition gradient)
        {
            var result = new GradientResult(Steps)
            {
                Angle = gradient.Angle
            };

            for (var i = 0; i < gradient.Steps; i++)
            {
                result.Gradient[i] = new byte[]
                {
                  (byte)Math.Floor(gradient.Red[i]),
                  (byte)Math.Floor(gradient.Green[i]),
                  (byte)Math.Floor(gradient.Blue[i]),
                  (byte)Math.Floor(gradient.Alpha[i])
                };
            }

            return result;
        }

        private GradientWorkingDefinition GetGradientDefinition(Image<Rgba32> img)
        {
            var gradient = new GradientWorkingDefinition(Steps, Angle);
            var resized = img.Clone(x => x.Resize(Steps, Steps));
            var cos = Math.Cos(Angle / 180.0 * Math.PI);
            var sin = Math.Sin(Angle / 180.0 * Math.PI);
            var fsteps = gradient.Steps;
            var hsteps = fsteps * 0.5;

            for (int y = 0; y < Steps; y++)
            {
                for (int x = 0; x < Steps; x++)
                {

                    for (var i = 0; i < Steps; i++)
                    {
                        var weight = fmod(sin * x + cos * y - i + hsteps, fsteps) - hsteps;
                        weight = 1.0 - Math.Abs(weight);
                        if (weight <= 0)
                        {
                            continue;
                        }

                        var pixel = resized[x, y];

                        gradient.Red[i] += pixel.R * weight;
                        gradient.Green[i] += pixel.G * weight;
                        gradient.Blue[i] += pixel.B * weight;
                        gradient.Alpha[i] += pixel.A * weight;
                        gradient.Unit[i] += weight;
                    }
                }

            }

            return gradient;
        }

        private void NormalizeGradient(GradientWorkingDefinition gradient)
        {
            // divide by unit
            for (var i = 0; i < gradient.Steps; i++)
            {
                var unit = gradient.Unit[i];
                gradient.Red[i] /= unit;
                gradient.Green[i] /= unit;
                gradient.Blue[i] /= unit;
                gradient.Alpha[i] /= unit;
                gradient.Unit[i] = 1;
            }
        }

        private double fmod(double a, double b)
        {
            return ((a - (Math.Floor(a / b) * b)));
        }
    }
}