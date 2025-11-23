namespace MassImageEditor.Core.Processors;

public class BrightnessProcessor : IImageProcessor
{
    private readonly int _brightness;

    public Bitmap Process(Bitmap image)
    {
        Bitmap adjustedImage = new Bitmap(image.Width, image.Height);

        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                Color pixel = image.GetPixel(x, y);

                int red = Math.Max(0, Math.Min(255, pixel.R + _brightness));
                int green = Math.Max(0, Math.Min(255, pixel.G + _brightness));
                int blue = Math.Max(0, Math.Min(255, pixel.B + _brightness));

                adjustedImage.SetPixel(x, y, Color.FromArgb(pixel.A, red, green, blue));
            }
        }

        return adjustedImage;
    }

    public BrightnessProcessor(int brightness)
    {
        _brightness = brightness;
    }

    public bool ShouldProcess => _brightness >= 0;
}