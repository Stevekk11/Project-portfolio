using System.Collections.Concurrent;

namespace MassImageEditor.Core;

/// <summary>
/// Represents a queue to manage image processing tasks in a thread-safe manner.
/// </summary>
/// <remarks>
/// This queue is designed to coordinate the execution of tasks involving
/// processing image files. It uses a bounded blocking collection
/// underneath, providing thread-safe operations for adding and retrieving tasks.
/// The bounded capacity limits the maximum number of items that can be enqueued.
/// </remarks>
public sealed class ImageTaskQueue
{
    private readonly BlockingCollection<ImageTask> _queue;

    public ImageTaskQueue(int boundedCapacity = 100)
    {
        _queue = new BlockingCollection<ImageTask>(boundedCapacity);
    }

    public void Enqueue(ImageTask task)
    {
        _queue.Add(task);
    }

    public ImageTask Dequeue(CancellationToken cancellationToken)
    {
        return _queue.Take(cancellationToken);
    }

    public void CompleteAdding()
    {
        _queue.CompleteAdding();
    }

    public bool IsCompleted => _queue.IsCompleted;
}