namespace MassImageEditor.Core.Processors;

/// <summary>
/// Processes images by resizing them to specified dimensions while maintaining aspect ratio.
/// Adds black letterboxing if the aspect ratio doesn't match.
/// </summary>
public sealed class ResizeProcessor : IImageProcessor
{
    private readonly int _targetWidth;
    private readonly int _targetHeight;

    public ResizeProcessor(int targetWidth, int targetHeight)
    {
        _targetWidth = targetWidth;
        _targetHeight = targetHeight;
    }

    public bool ShouldProcess => _targetWidth > 0 && _targetHeight > 0;

    public Bitmap Process(Bitmap image)
    {
        if (!ShouldProcess)
            return image;

        // Calculate aspect ratios
        double sourceRatio = (double)image.Width / image.Height;
        double targetRatio = (double)_targetWidth / _targetHeight;

        int drawWidth, drawHeight, offsetX, offsetY;

        if (sourceRatio > targetRatio)
        {
            // Source is wider - fit to width, add bars top/bottom
            drawWidth = _targetWidth;
            drawHeight = (int)(_targetWidth / sourceRatio);
            offsetX = 0;
            offsetY = (_targetHeight - drawHeight) / 2;
        }
        else
        {
            // Source is taller - fit to height, add bars left/right
            drawWidth = (int)(_targetHeight * sourceRatio);
            drawHeight = _targetHeight;
            offsetX = (_targetWidth - drawWidth) / 2;
            offsetY = 0;
        }

        var resized = new Bitmap(_targetWidth, _targetHeight);
        using (var graphics = Graphics.FromImage(resized))
        {
            // Fill with black background
            graphics.Clear(Color.Black);

            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            graphics.DrawImage(image, offsetX, offsetY, drawWidth, drawHeight);
        }

        return resized;
    }
}

