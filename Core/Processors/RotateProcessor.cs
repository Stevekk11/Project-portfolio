namespace MassImageEditor.Core.Processors;

/// <summary>
/// Processes images by rotating them by specified degrees.
/// Supports 90, 180, 270, -90, -180, -270 degree rotations.
/// </summary>
public sealed class RotateProcessor : IImageProcessor
{
    private readonly int _degrees;

    public RotateProcessor(int degrees)
    {
        _degrees = degrees;
    }

    public bool ShouldProcess => _degrees != 0;

    public Bitmap Process(Bitmap image)
    {
        if (!ShouldProcess)
            return image;

        // Normalize degrees to 0-360 range
        int normalizedDegrees = ((_degrees % 360) + 360) % 360;

        if (normalizedDegrees == 0)
            return image;

        var rotated = new Bitmap(image);

        if (normalizedDegrees == 90)
        {
            rotated.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }
        else if (normalizedDegrees == 180)
        {
            rotated.RotateFlip(RotateFlipType.Rotate180FlipNone);
        }
        else if (normalizedDegrees == 270)
        {
            rotated.RotateFlip(RotateFlipType.Rotate270FlipNone);
        }

        return rotated;
    }
}
