namespace MassImageEditor.Core.Processors;

/// <summary>
/// Interface for image processing operations.
/// </summary>
public interface IImageProcessor
{
    /// <summary>
    /// Processes the given bitmap and returns a new processed bitmap.
    /// </summary>
    /// <param name="image">The input image to process</param>
    /// <returns>The processed image (may be the same instance if no processing needed)</returns>
    Bitmap Process(Bitmap image);

    /// <summary>
    /// Whether this processor should be applied based on current settings.
    /// </summary>
    bool ShouldProcess { get; }
}
