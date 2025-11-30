using MassImageEditor.Core;

namespace MassImageEditor;

/// <summary>
/// Represents a settings window for configuring application-specific processing options.
/// </summary>
/// <remarks>
/// This class is a partial class that extends the Form class. It serves as a user interface
/// to load and manage processing settings using the singleton instance of <see cref="ProcessingSettings"/>.
/// The window is initialized with default settings and provides a method for loading the settings.
/// </remarks>
public partial class Settings : Form
{
    private readonly ProcessingSettings _settings;

    public Settings()
    {
        InitializeComponent();
        DetectThreads();
        _settings = ProcessingSettings.Instance;
        LoadSettings();
    }

    /// <summary>
    /// Loads the application settings for image processing into the corresponding UI components,
    /// such as checkboxes, text boxes, combo boxes, and numeric up-down controls.
    /// This method reflects the current configuration stored within the ProcessingSettings instance
    /// into the UI, enabling the user to view and potentially modify these settings.
    /// </summary>
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
        //Black and white checkbox
        BlackAndWhiteBox.Checked = _settings.BlackAndWhiteEnabled;
        // Brightness checkbox and value
        BrightnessBox.Checked = _settings.BrightnessEnabled;
        trackBar1.Value = _settings.BrightnessValue;
        ContrastTrackBar.Value = _settings.ContrastValue;
    }

    /// <summary>
    /// Saves the application settings related to image processing from the UI components
    /// into the corresponding properties of the ProcessingSettings instance.
    /// This method captures the current state of the UI, ensuring that user-defined
    /// configuration changes are preserved for future operations.
    /// </summary>
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
        //save black and white checkbox
        _settings.BlackAndWhiteEnabled = BlackAndWhiteBox.Checked;
        //Save brightness checkbox and value
        _settings.BrightnessEnabled = BrightnessBox.Checked;
        _settings.BrightnessValue = trackBar1.Value;
        //Save contrast checkbox and value
        _settings.ContrastValue = ContrastTrackBar.Value;
        _settings.ContrastEnabled = ContrastCheckBox.Checked;
    }

    private bool ValidateDimensions(int? w, int? h)
    {
        if (_settings.ResizeEnabled)
            return w > 0 && h > 0;
        return true;
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
        if (ValidateDimensions(_settings.Width, _settings.Height))
        {
            Close();
        }
        else
        {
            MessageBox.Show("Width and height must be greater than zero.", "Invalid dimensions", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
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

    private void BrightnessBox_CheckedChanged(object sender, EventArgs e)
    {
        if (BrightnessBox.Checked)
        {
            trackBar1.Enabled = true;
        }
        else
        {
            trackBar1.Enabled = false;
        }
    }

    public void DetectThreads()
    {
        int processorCount = Environment.ProcessorCount;
        MaxThreadsChooser.Maximum = processorCount;
        MaxThreadsChooser.Value = processorCount;
    }

    private void ContrastCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        if (ContrastCheckBox.Checked)
        {
            ContrastTrackBar.Enabled = true;
        }
        else
        {
            ContrastTrackBar.Enabled = false;
        }
    }
}