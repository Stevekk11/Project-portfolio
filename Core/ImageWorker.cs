using MassImageEditor.Core.Processors;
using Serilog;

namespace MassImageEditor.Core;

public sealed class ImageWorker
{
    private readonly ImageTaskQueue _taskQueue;
    private readonly CancellationToken _cancellationToken;
    private readonly ImageProcessorPipeline _pipeline;
    private readonly FormatSaver _formatSaver;

    public event Action<ImageTask>? TaskCompleted;
    public event Action<ImageTask>? TaskStarted;
    public event Action<ImageTask, Exception>? TaskFailed;

    public Func<ImageTask, IProgressReporter?>? ProgressReporterFactory { get; set; }

    public ImageWorker(ImageTaskQueue taskQueue, CancellationToken cancellationToken)
    {
        _taskQueue = taskQueue;
        _cancellationToken = cancellationToken;

        var settings = ProcessingSettings.Instance;
        _pipeline = ImageProcessorPipeline.CreateFromSettings(settings);
        _formatSaver = new FormatSaver(settings.ConvertEnabled ? settings.ConvertToFormat ?? "" : "");
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
            ImageTask? task;

            try
            {
                task = _taskQueue.Dequeue(_cancellationToken);

            }
            catch (OperationCanceledException e)
            {
                Log.Information("ImageWorker loop cancelled");
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

                var reporter = ProgressReporterFactory?.Invoke(task);
                ProcessImage(task, reporter);

                TaskCompleted?.Invoke(task);
            }
            catch (Exception ex)
            {
                TaskFailed?.Invoke(task, ex);
            }
        }
    }

    /// <summary>
    /// Processes an image task by applying transformations through the processor pipeline
    /// and saving the result to the output path.
    /// </summary>
    /// <param name="task">The image task that encapsulates the input file path and output file path for processing.</param>
    /// <param name="progressReporter">Reports the progress of the task.</param>
    private void ProcessImage(ImageTask task, IProgressReporter? progressReporter = null)
    {
        // Load the image
        using var image = Image.FromFile(task.FilePath);
        using var bitmap = new Bitmap(image);

        Bitmap processedImage = _pipeline.Process(bitmap, progressReporter);

        // Determine output path with correct extension
        string outputPath = _formatSaver.GetOutputPath(task.OutputPath);

        // Save the processed image
        _formatSaver.Save(processedImage, outputPath);

        // Clean up if we created a new bitmap
        if (processedImage != bitmap)
        {
            processedImage.Dispose();
        }
    }

}