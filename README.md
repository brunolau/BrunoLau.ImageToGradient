# ImageToGradient

Simple library generating linear gradient based on given image. It's a .NET port of https://github.com/peterekepeter/image-to-gradient#readme library

## Feeds

* NuGet [![NuGet](https://img.shields.io/nuget/vpre/BrunoLau.ImageToGradient.svg)](https://www.nuget.org/profiles/BrunoLau.ImageToGradient)

## Sample usage
```
const string IMAGE_URL = "https://placekitten.com/408/287";
var processor = new BrunoLau.ImageToGradient.ImageToGradientConverter()
{
    Angle = 60,
    Steps = 10
};

var gradient = await processor.ProcessUrlAsync(IMAGE_URL);
Console.WriteLine(gradient.BuildCssGradient());
Console.ReadKey();
```