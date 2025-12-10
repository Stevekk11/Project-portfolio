using Serilog;

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
    /// Processes an image through all processors in the pipeline, reporting progress.
    /// Returns the final processed image.
    /// </summary>
    public Bitmap Process(Bitmap sourceImage, IProgressReporter? progressReporter)
    {
        Bitmap currentImage = sourceImage;
        Bitmap? previousImage = null;

        foreach (var processor in _processors)
        {
            if (!processor.ShouldProcess)
                continue;

            Bitmap processedImage = processor.Process(currentImage, progressReporter);

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
            Log.Information("Resize processor added: {Width}x{Height}", settings.Width.Value, settings.Height.Value);
        }

        // Add rotate processor
        if (settings.RotateEnabled && settings.RotationDegrees != 0)
        {
            pipeline.AddProcessor(new RotateProcessor(settings.RotationDegrees));
            Log.Information("Rotate processor added: {Degrees} degrees", settings.RotationDegrees);
        }
        //Add black and white processor
        if (settings.BlackAndWhiteEnabled) { pipeline.AddProcessor(new BlackAndWhiteProcessor(true)); Log.Information("Black and white processor added"); }
        //Add brightness processor
        if (settings.BrightnessEnabled)
        {
            pipeline.AddProcessor(new BrightnessProcessor(settings.BrightnessValue));
            Log.Information("Brightness processor added: {Value}", settings.BrightnessValue);
        }
        //Add contrast processor
        if (settings.ContrastEnabled)
        {
            pipeline.AddProcessor(new ContrastProcessor(settings.ContrastValue));
            Log.Information("Contrast processor added: {Value}", settings.ContrastValue);
        }
        //Add sharpness processor
        if (settings.SharpnessEnabled)
        {
            pipeline.AddProcessor(new SharpnessProcessor(settings.SharpnessValue));
            Log.Information("Sharpness processor added: {Value}", settings.SharpnessValue);
        }
        //add pixelate processor
        if (settings.PixelateEnabled)
        {
            pipeline.AddProcessor(new PixelateProcessor(settings.PixelateBlockSize));
            Log.Information("Pixelate processor added: {BlockSize}", settings.PixelateBlockSize);
        }

        return pipeline;
    }
}
