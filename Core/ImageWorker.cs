namespace MassImageEditor.Core;

public sealed class ImageWorker
{
    private readonly ImageTaskQueue _taskQueue;
    private readonly CancellationToken _cancellationToken;

    public event Action<ImageTask>? TaskCompleted;
    public event Action<ImageTask>? TaskStarted;
    public event Action<ImageTask, Exception>? TaskFailed;

    public ImageWorker(ImageTaskQueue taskQueue, CancellationToken cancellationToken)
    {
        _taskQueue = taskQueue;
        _cancellationToken = cancellationToken;
    }

    public Task StartAsync()
    {
        return Task.Run(RunWorkerLoop, _cancellationToken);
    }

    /// <summary>
    /// Continuously processes image tasks from the task queue until the cancellation is requested or the queue is marked as completed.
    /// During task execution, the following events may be triggered:
    /// <list type="bullet">
    /// <item>
    /// <description><c>TaskStarted</c> - Invoked when a new task begins processing.</description>
    /// </item>
    /// <item>
    /// <description><c>TaskCompleted</c> - Invoked when a task has been successfully processed.</description>
    /// </item>
    /// <item>
    /// <description><c>TaskFailed</c> - Invoked if an exception occurs during task processing.</description>
    /// </item>
    /// </list>
    /// Tasks are dequeued from the <see cref="ImageTaskQueue"/>. If the queue is empty or completed, and
    /// cancellation via the <see cref="CancellationToken"/> has not been requested, the loop terminates.
    /// </summary>
    private void RunWorkerLoop()
    {
        while (!_taskQueue.IsCompleted && !_cancellationToken.IsCancellationRequested)
        {
            ImageTask? task = null;

            try
            {
                task = _taskQueue.Dequeue(_cancellationToken);

            }
            catch (OperationCanceledException e)
            {
                break;
            }
            catch(InvalidOperationException)
            {
                //Queue is empty
                break;
            }

            if (task == null)
                continue;

            try
            {
                TaskStarted?.Invoke(task);

                // TODO: Replace this with real image processing logic.
                ProcessImage(task);

                TaskCompleted?.Invoke(task);
            }
            catch (Exception ex)
            {
                TaskFailed?.Invoke(task, ex);
            }
        }
    }

    /// <summary>
    /// Processes an image task by applying transformations such as resizing, rotation, and format conversion
    /// based on the defined settings in <see cref="ProcessingSettings"/>. The processed image is saved
    /// to the specified output path.
    /// </summary>
    /// <param name="task">The image task that encapsulates the input file path and output file path for processing.</param>
    private void ProcessImage(ImageTask task)
    {
        var settings = ProcessingSettings.Instance;

        // Load the image
        using var image = Image.FromFile(task.FilePath);
        using var bitmap = new Bitmap(image);

        Bitmap processedImage = bitmap;

        // Apply resize if enabled
        if (settings.ResizeEnabled && settings.Width.HasValue && settings.Height.HasValue)
        {
            processedImage = ResizeImage(processedImage, settings.Width.Value, settings.Height.Value);
        }

        // Apply rotation if enabled
        if (settings.RotateEnabled && settings.RotationDegrees != 0)
        {
            processedImage = RotateImage(processedImage, settings.RotationDegrees);
        }

        // Save with format conversion if needed
        string outputPath = task.OutputPath;
        if (settings.ConvertEnabled && !string.IsNullOrEmpty(settings.ConvertToFormat))
        {
            outputPath = Path.ChangeExtension(task.OutputPath, settings.ConvertToFormat.ToLower());
            SaveImageWithFormat(processedImage, outputPath, settings.ConvertToFormat);
        }
        else
        {
            processedImage.Save(outputPath);
        }

        // Clean up if we created a new bitmap
        if (processedImage != bitmap)
        {
            processedImage.Dispose();
        }
    }

    private Bitmap ResizeImage(Bitmap image, int targetWidth, int targetHeight)
    {
        // Calculate aspect ratios
        double sourceRatio = (double)image.Width / image.Height;
        double targetRatio = (double)targetWidth / targetHeight;

        int drawWidth, drawHeight, offsetX, offsetY;

        if (sourceRatio > targetRatio)
        {
            // Source is wider - fit to width, add bars top/bottom
            drawWidth = targetWidth;
            drawHeight = (int)(targetWidth / sourceRatio);
            offsetX = 0;
            offsetY = (targetHeight - drawHeight) / 2;
        }
        else
        {
            // Source is taller - fit to height, add bars left/right
            drawWidth = (int)(targetHeight * sourceRatio);
            drawHeight = targetHeight;
            offsetX = (targetWidth - drawWidth) / 2;
            offsetY = 0;
        }

        var resized = new Bitmap(targetWidth, targetHeight);
        using (var graphics = Graphics.FromImage(resized))
        {
            // Fill with black bars
            graphics.Clear(Color.Black);

            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            graphics.DrawImage(image, offsetX, offsetY, drawWidth, drawHeight);
        }
        return resized;
    }

    private Bitmap RotateImage(Bitmap image, int degrees)
    {
        // Normalize degrees to 0-360 range
        degrees = ((degrees % 360) + 360) % 360;

        if (degrees == 0)
            return image;

        var rotated = new Bitmap(image);

        if (degrees == 90)
        {
            rotated.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }
        else if (degrees == 180 || degrees == -180)
        {
            rotated.RotateFlip(RotateFlipType.Rotate180FlipNone);
        }
        else if (degrees == 270 || degrees == -90)
        {
            rotated.RotateFlip(RotateFlipType.Rotate270FlipNone);
        }

        return rotated;
    }

    private void SaveImageWithFormat(Bitmap image, string path, string format)
    {
        System.Drawing.Imaging.ImageFormat imageFormat = format.ToUpper() switch
        {
            "JPG" or "JPEG" => System.Drawing.Imaging.ImageFormat.Jpeg,
            "PNG" => System.Drawing.Imaging.ImageFormat.Png,
            "BMP" => System.Drawing.Imaging.ImageFormat.Bmp,
            "WEBP" => System.Drawing.Imaging.ImageFormat.Webp,
            _ => System.Drawing.Imaging.ImageFormat.Png
        };

        image.Save(path, imageFormat);
    }

}