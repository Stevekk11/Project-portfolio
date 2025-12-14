using System.Drawing.Imaging;

namespace MassImageEditor.Core.Processors;

/// <summary>
/// Changes the contrast of the given image.
///
/// <returns></returns>
/// </summary>
public class ContrastProcessor : IImageProcessor
{
    private int _contrast;

    public ContrastProcessor(int contrast)
    {
        _contrast = contrast;
    }


    public Bitmap Process(Bitmap image)
    {
        if (!ShouldProcess)
        {
            return image;
        }

        //  - Square the value to make adjustments more perceptually linear for larger values.
        float contrast = (100.0f + _contrast) / 100.0f;
        contrast *= contrast;

        // Create a new bitmap to draw the adjusted image onto
        Bitmap adjustedImage = new Bitmap(image.Width, image.Height);

        // Create ImageAttributes and ColorMatrix
        using (Graphics g = Graphics.FromImage(adjustedImage))
        {
            // ImageAttributes allows us to apply a color transformation when drawing.
            ImageAttributes attributes = new ImageAttributes();
            // Construct a ColorMatrix to apply the contrast change to RGB channels.
            // The matrix rows correspond to:
            //  - Row 0: red multipliers and offset
            //  - Row 1: green multipliers and offset
            //  - Row 2: blue multipliers and offset
            //  - Row 3: alpha multipliers (leave alpha unchanged)
            //  - Row 4: translation vector (offsets applied to R,G,B,A; last element is 1)
            //
            // We multiply each color channel by the same contrast factor to preserve color balance.
            // The offset row (last row in this matrix representation) shifts colors around the midpoint
            // so that contrast adjustments are applied relative to the image mid-tone.
            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][]
                {
                    new float[] { contrast, 0, 0, 0, 0 }, // Red channel scale
                    new float[] { 0, contrast, 0, 0, 0 }, // Green channel scale
                    new float[] { 0, 0, contrast, 0, 0 }, // Blue channel scale
                    new float[] { 0, 0, 0, 1, 0 }, // Alpha channel (unchanged)
                    // Translation offsets: shift each color toward the midpoint (0.5)
                    // Formula: 0.5 * (1 - contrast) ensures the mid-tone remains stable.
                    new float[] { 0.5f * (1 - contrast), 0.5f * (1 - contrast), 0.5f * (1 - contrast), 0, 1 }
                });

            attributes.SetColorMatrix(colorMatrix);

            // Draw the original image with the color matrix
            // The destination rectangle covers the whole adjusted image.
            // GraphicsUnit.Pixel indicates coordinates are in pixels.
            g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                0, 0, image.Width, image.Height,
                GraphicsUnit.Pixel, attributes);
        }

        return adjustedImage;
    }

    public bool ShouldProcess => _contrast < 101 && _contrast > -101;
}