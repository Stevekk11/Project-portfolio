namespace MassImageEditor.Core.Processors;

/// <summary>
/// Interface for image processing operations.
/// </summary>
public interface IImageProcessor
{
    /// <summary>
    /// Processes the given bitmap and returns a new processed bitmap.
    /// Implementers must provide this simple, progress-less method.
    /// </summary>
    Bitmap Process(Bitmap image);

    /// <summary>
    /// Optional progress-aware overload. Default implementation calls the simple method for backward compatibility.
    /// </summary>
    Bitmap Process(Bitmap image, IProgressReporter? progressReporter)
    {
        return Process(image);
    }

    /// <summary>
    /// Whether this processor should be applied based on current settings.
    /// </summary>
    bool ShouldProcess { get; }
}
