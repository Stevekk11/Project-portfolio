namespace MassImageEditor.Core.Processors;

/// <summary>
/// Manages a pipeline of image processors that are applied in sequence.
/// </summary>
public sealed class ImageProcessorPipeline
{
    private readonly List<IImageProcessor> _processors = new();

    /// <summary>
    /// Adds a processor to the pipeline.
    /// </summary>
    public void AddProcessor(IImageProcessor processor)
    {
        _processors.Add(processor);
    }

    /// <summary>
    /// Processes an image through all processors in the pipeline.
    /// Returns the final processed image.
    /// </summary>
    public Bitmap Process(Bitmap sourceImage)
    {
        Bitmap currentImage = sourceImage;
        Bitmap? previousImage = null;

        foreach (var processor in _processors)
        {
            if (!processor.ShouldProcess)
                continue;

            Bitmap processedImage = processor.Process(currentImage);

            // Clean up intermediate images (but not the source)
            if (previousImage != null && previousImage != sourceImage)
            {
                previousImage.Dispose();
            }

            previousImage = currentImage;
            currentImage = processedImage;
        }

        return currentImage;
    }

    /// <summary>
    /// Creates a pipeline based on the current ProcessingSettings.
    /// </summary>
    public static ImageProcessorPipeline CreateFromSettings(ProcessingSettings settings)
    {
        var pipeline = new ImageProcessorPipeline();

        // Add resize processor
        if (settings.ResizeEnabled && settings.Width.HasValue && settings.Height.HasValue)
        {
            pipeline.AddProcessor(new ResizeProcessor(settings.Width.Value, settings.Height.Value));
        }

        // Add rotate processor
        if (settings.RotateEnabled && settings.RotationDegrees != 0)
        {
            pipeline.AddProcessor(new RotateProcessor(settings.RotationDegrees));
        }
        //Add black and white processor
        pipeline.AddProcessor(new BlackAndWhiteProcessor(settings.BlackAndWhiteEnabled));
        //Add brightness processor
        if (settings.BrightnessEnabled)
        {
            pipeline.AddProcessor(new BrightnessProcessor(settings.BrightnessValue));
        }
        //Add contrast processor
        if (settings.ContrastEnabled)
        {
            pipeline.AddProcessor(new ContrastProcessor(settings.ContrastValue));
        }

        return pipeline;
    }
}
