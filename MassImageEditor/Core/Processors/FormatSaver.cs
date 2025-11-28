namespace MassImageEditor.Core.Processors;

/// <summary>
/// Handles saving images in different formats.
/// </summary>
public sealed class FormatSaver
{
    private readonly string _format;

    public FormatSaver(string format)
    {
        _format = format;
    }

    public bool ShouldConvert => !string.IsNullOrEmpty(_format);

    /// <summary>
    /// Saves the image to the specified path with the configured format.
    /// </summary>
    public void Save(Bitmap image, string path)
    {
        if (!ShouldConvert)
        {
            image.Save(path);
            return;
        }

        System.Drawing.Imaging.ImageFormat imageFormat = _format.ToUpper() switch
        {
            "JPG" or "JPEG" => System.Drawing.Imaging.ImageFormat.Jpeg,
            "PNG" => System.Drawing.Imaging.ImageFormat.Png,
            "BMP" => System.Drawing.Imaging.ImageFormat.Bmp,
            "WEBP" => System.Drawing.Imaging.ImageFormat.Webp,
            _ => System.Drawing.Imaging.ImageFormat.Png
        };

        image.Save(path, imageFormat);
    }

    /// <summary>
    /// Gets the output path with the correct extension based on format.
    /// </summary>
    public string GetOutputPath(string originalPath)
    {
        if (!ShouldConvert)
            return originalPath;

        return Path.ChangeExtension(originalPath, _format.ToLower());
    }
}
