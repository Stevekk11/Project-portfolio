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