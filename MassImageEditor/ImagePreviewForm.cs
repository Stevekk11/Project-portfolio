using Serilog;

namespace MassImageEditor;

public partial class ImagePreviewForm : Form
{
    private readonly PictureBox _pictureBox;
    public ImagePreviewForm(string imgPath)
    {
        Text = Path.GetFileName(imgPath);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.Sizable;
        Width = 800;
        Height = 600;

        _pictureBox = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
        };
        Controls.Add(_pictureBox);
        LoadImage(imgPath);
        InitializeComponent();
    }

    private void LoadImage(string imgPath)
    {
        try
        {
            using var img = Image.FromFile(imgPath);
            _pictureBox.Image = new Bitmap(img);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load image: {ex.Message}, {imgPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Log.Error($"Failed to load image {imgPath}: {ex.Message}");
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _pictureBox.Image?.Dispose();
            _pictureBox.Dispose();
        }
        base.Dispose(disposing);
    }
}