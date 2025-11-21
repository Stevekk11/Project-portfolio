using MassImageEditor.Core;

namespace MassImageEditor;

public partial class MainWindow : Form
{
    private string? _inputFolder;
    private string? _outputFolder;
    private ImageTaskQueue? _taskQueue;
    private List<ImageWorker> _workers = new();
    private CancellationTokenSource? _cancellationTokenSource;

    public MainWindow()
    {
        InitializeComponent();

        // Wire up event handlers
        InputButton.Click += InputButton_Click;
        OutputButton.Click += OutputButton_Click;
        StartButton.Click += StartButton_Click;
        ClearButton.Click += ClearButton_Click;
        CancelButton1.Click += CancelButton_Click;

        Images.View = View.Details;
        Images.Columns.Clear();
        Images.Columns.Add("File Name", 200);
        Images.Columns.Add("Status", 100);
        Images.Columns.Add("Progress", 80);
        Images.Columns.Add("Message", 250);
    }

    private void SettingsButton_Click(object sender, EventArgs e)
    {
        Settings settings = new Settings();
        settings.ShowDialog();
    }

    private void InputButton_Click(object? sender, EventArgs e)
    {
        using var folderDialog = new FolderBrowserDialog();
        folderDialog.Description = "Select folder containing images";

        if (folderDialog.ShowDialog() == DialogResult.OK)
        {
            _inputFolder = folderDialog.SelectedPath;
            LoadImagesFromFolder(_inputFolder);
        }
    }

    private void OutputButton_Click(object? sender, EventArgs e)
    {
        using var folderDialog = new FolderBrowserDialog();
        folderDialog.Description = "Select output folder for processed images";

        if (folderDialog.ShowDialog() == DialogResult.OK)
        {
            _outputFolder = folderDialog.SelectedPath;
        }
    }

    private void StartButton_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_inputFolder) || string.IsNullOrEmpty(_outputFolder))
        {
            MessageBox.Show("Please select both input and output folders.", "Missing Folders",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (Images.Items.Count == 0)
        {
            MessageBox.Show("No images to process.", "No Images",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        StartProcessing();
    }

    private void ClearButton_Click(object? sender, EventArgs e)
    {
        Images.Items.Clear();
        _inputFolder = null;
        _outputFolder = null;
        progressBar1.Value = 0;
    }

    private void CancelButton_Click(object? sender, EventArgs e)
    {
        _cancellationTokenSource?.Cancel();
    }

    private void LoadImagesFromFolder(string folderPath)
    {
        Images.Items.Clear();

        string[] extensions = { "*.jpg", "*.jpeg", "*.png", "*.bmp", "*.gif", "*.webp" };
        var imageFiles = extensions.SelectMany(ext =>
            Directory.GetFiles(folderPath, ext, SearchOption.AllDirectories)).ToArray();

        foreach (var file in imageFiles)
        {
            var item = new ListViewItem(Path.GetFileName(file));
            item.SubItems.Add("Pending");
            item.SubItems.Add("0%");
            item.SubItems.Add("");
            item.Tag = file;
            Images.Items.Add(item);
        }
    }

    /// <summary>
    /// Initiates the image processing workflow by preparing a task queue, creating worker threads,
    /// and handling the processing of each image in the input folder list. The progress bar will
    /// display the progress as the tasks are completed. Event handlers are attached to monitor the
    /// status of individual tasks (started, completed, or failed). This method executes asynchronously.
    /// </summary>
    /// <remarks>
    /// This method will verify that both input and output folders are specified, and that there are
    /// images to process. It utilizes a cancellation token to handle potential interruptions during
    /// the processing workflow. The number of worker threads is determined by the MaxThreads setting
    /// in the ProcessingSettings configuration.
    /// </remarks>
    private async void StartProcessing()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _taskQueue = new ImageTaskQueue(Images.Items.Count);
        _workers.Clear();

        progressBar1.Maximum = Images.Items.Count;
        progressBar1.Value = 0;

        // Enqueue all tasks
        foreach (ListViewItem item in Images.Items)
        {
            string inputPath = (string)item.Tag!;
            string fileName = Path.GetFileName(inputPath);
            string outputPath = Path.Combine(_outputFolder!, fileName);

            var task = new ImageTask(inputPath, outputPath);
            _taskQueue.Enqueue(task);
        }

        _taskQueue.CompleteAdding();

        // Create workers based on settings
        int workerCount = ProcessingSettings.Instance.MaxThreads;
        for (int i = 0; i < workerCount; i++)
        {
            var worker = new ImageWorker(_taskQueue, _cancellationTokenSource.Token);
            worker.TaskStarted += OnTaskStarted;
            worker.TaskCompleted += OnTaskCompleted;
            worker.TaskFailed += OnTaskFailed;
            _workers.Add(worker);
        }

        // Start all workers
        var workerTasks = _workers.Select(w => w.StartAsync()).ToArray();
        await Task.WhenAll(workerTasks);
    }

    /// <summary>
    /// Handles the event signaling that an image task has started processing.
    /// Updates the corresponding ListView item to reflect the "Processing" status.
    /// </summary>
    /// <param name="task">The image task that has started processing, containing details such as input and output file paths.</param>
    private void OnTaskStarted(ImageTask task)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<ImageTask>(OnTaskStarted), task);
            return;
        }

        var item = FindListViewItem(task.FilePath);
        if (item != null)
        {
            item.SubItems[1].Text = "Processing";
        }
    }

    /// <summary>
    /// Handles the completion of an image processing task by updating the UI to reflect the completed status,
    /// including the task's progress, status, and success message. The progress bar is incremented to represent
    /// the completion of the task.
    /// </summary>
    /// <param name="task">The image task that has been completed. Contains information about the file paths for the input image and output image.</param>
    private void OnTaskCompleted(ImageTask task)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<ImageTask>(OnTaskCompleted), task);
            return;
        }

        var item = FindListViewItem(task.FilePath);
        if (item != null)
        {
            item.SubItems[1].Text = "Completed";
            item.SubItems[2].Text = "100%";
            item.SubItems[3].Text = "Success";
        }

        progressBar1.Value++;
    }

    private void OnTaskFailed(ImageTask task, Exception ex)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<ImageTask, Exception>(OnTaskFailed), task, ex);
            return;
        }

        var item = FindListViewItem(task.FilePath);
        if (item != null)
        {
            item.SubItems[1].Text = "Failed";
            item.SubItems[3].Text = ex.Message;
        }

        progressBar1.Value++;
    }

    /// <summary>
    /// Searches the ListView for an item that matches the specified file path.
    /// This method iterates through all the items in the ListView and checks their tags for a match.
    /// </summary>
    /// <param name="filePath">The file path to search for within the ListView items' tags.</param>
    /// <returns>
    /// The ListViewItem that matches the specified file path, or null if no match is found.
    /// </returns>
    private ListViewItem? FindListViewItem(string filePath)
    {
        foreach (ListViewItem item in Images.Items)
        {
            if ((string)item.Tag! == filePath)
                return item;
        }
        return null;
    }
}