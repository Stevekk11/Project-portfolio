using System.Drawing.Imaging;

namespace MassImageEditor.Core.Processors;

public class ContrastProcessor : IImageProcessor
{
    private int _contrast;

    public ContrastProcessor(int contrast)
    {
        _contrast = contrast;
    }

    /// <summary>
    /// Changes the contrast of the given image.
    ///
    /// </summary>
    /// <param name="image">The image to process</param>
    /// <returns></returns>
    public Bitmap Process(Bitmap image)
    {
        if (!ShouldProcess)
        {
            return image;
        }
        float contrast = (100.0f + _contrast) / 100.0f;
        contrast *= contrast;

        // Create a new bitmap to draw the adjusted image onto
        Bitmap adjustedImage = new Bitmap(image.Width, image.Height);

        // Create ImageAttributes and ColorMatrix
        using (Graphics g = Graphics.FromImage(adjustedImage))
        {
            ImageAttributes attributes = new ImageAttributes();
            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][]
                {
                    new float[] { contrast, 0, 0, 0, 0 },
                    new float[] { 0, contrast, 0, 0, 0 },
                    new float[] { 0, 0, contrast, 0, 0 },
                    new float[] { 0, 0, 0, 1, 0 },
                    // The offset to shift colors around the midpoint (0.5 or 128)
                    new float[] { 0.5f * (1 - contrast), 0.5f * (1 - contrast), 0.5f * (1 - contrast), 0, 1 }
                });

            attributes.SetColorMatrix(colorMatrix);

            // Draw the original image with the color matrix
            g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                0, 0, image.Width, image.Height,
                GraphicsUnit.Pixel, attributes);
        }

        return adjustedImage;
    }

    public bool ShouldProcess => _contrast < 101 && _contrast > -101;
}