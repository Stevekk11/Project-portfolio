using MassImageEditor.Core;

namespace MassImageEditor;

public partial class Settings : Form
{
    private readonly ProcessingSettings _settings;

    public Settings()
    {
        InitializeComponent();
        _settings = ProcessingSettings.Instance;
        LoadSettings();
    }

    private void LoadSettings()
    {
        // Load resize settings
        ResizeCheckBox.Checked = _settings.ResizeEnabled;
        WidthBox.Text = _settings.Width?.ToString() ?? "";
        HeigthBox.Text = _settings.Height?.ToString() ?? "";

        // Load rotation settings
        Rotate.Checked = _settings.RotateEnabled;
        if (_settings.RotationDegrees != 0)
        {
            RotateBox.Text = $"{_settings.RotationDegrees}°";
        }

        // Load conversion settings
        Convert.Checked = _settings.ConvertEnabled;
        if (!string.IsNullOrEmpty(_settings.ConvertToFormat))
        {
            ConvertBox.Text = _settings.ConvertToFormat;
        }

        // Load performance settings
        MaxThreadsChooser.Value = _settings.MaxThreads;
    }

    private void SaveSettings()
    {
        // Save resize settings
        _settings.ResizeEnabled = ResizeCheckBox.Checked;
        if (int.TryParse(WidthBox.Text, out int width))
        {
            _settings.Width = width;
        }
        if (int.TryParse(HeigthBox.Text, out int height))
        {
            _settings.Height = height;
        }

        // Save rotation settings
        _settings.RotateEnabled = Rotate.Checked;
        if (RotateBox.SelectedItem != null)
        {
            string rotation = RotateBox.SelectedItem.ToString()!.Replace("°", "");
            if (int.TryParse(rotation, out int degrees))
            {
                _settings.RotationDegrees = degrees;
            }
        }

        // Save conversion settings
        _settings.ConvertEnabled = Convert.Checked;
        if (ConvertBox.SelectedItem != null)
        {
            _settings.ConvertToFormat = ConvertBox.SelectedItem.ToString();
        }

        // Save performance settings
        _settings.MaxThreads = (int)MaxThreadsChooser.Value;
    }

    private void ResizeCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        if (ResizeCheckBox.Checked)
        {
            WidthBox.Enabled = true;
            HeigthBox.Enabled = true;
        }
        else
        {
            WidthBox.Enabled = false;
            HeigthBox.Enabled = false;
        }
    }

    private void Rotate_CheckedChanged(object sender, EventArgs e)
    {
        if (Rotate.Checked)
        {
            RotateBox.Enabled = true;
        }
        else
        {
            RotateBox.Enabled = false;
        }
    }

    private void ExitButton_Click(object sender, EventArgs e)
    {
        SaveSettings();
        this.Close();
    }

    private void Convert_CheckedChanged(object sender, EventArgs e)
    {
        if (Convert.Checked)
        {
            ConvertBox.Enabled = true;
        }
        else
        {
            ConvertBox.Enabled = false;
        }
    }
}