
using BrunoLau.ImageToGradient;

const string IMAGE_URL = "https://placekitten.com/408/287";
var processor = new ImageToGradientConverter()
{
    Angle = 60,
    Steps = 10
};

var gradient = await processor.ProcessUrlAsync(IMAGE_URL);
Console.WriteLine(gradient.BuildCssGradient());
Console.ReadKey();