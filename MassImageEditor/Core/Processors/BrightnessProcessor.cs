namespace MassImageEditor.Core.Processors;

/// <summary>
/// The BrightnessProcessor class is responsible for adjusting the brightness of an image.
/// It processes an input bitmap by increasing or decreasing the color intensity of each pixel
/// based on a specified brightness value.
/// </summary>
public class BrightnessProcessor : IImageProcessor
{
    private int _brightness;

    public BrightnessProcessor(int brightness)
    {
        _brightness = brightness;
    }

    public Bitmap Process(Bitmap image)
    {
        if (!ShouldProcess)
        {
            return image;
        }


        int adjustedBrightness = (int)(_brightness * 2.55);
        Bitmap adjustedImage = new Bitmap(image.Width, image.Height);

        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                Color pixel = image.GetPixel(x, y);

                int red = Math.Max(0, Math.Min(255, pixel.R + adjustedBrightness));
                int green = Math.Max(0, Math.Min(255, pixel.G + adjustedBrightness));
                int blue = Math.Max(0, Math.Min(255, pixel.B + adjustedBrightness));

                adjustedImage.SetPixel(x, y, Color.FromArgb(pixel.A, red, green, blue));
            }
        }

        return adjustedImage;
    }

    public bool ShouldProcess => _brightness < 101 && _brightness > -101;
}