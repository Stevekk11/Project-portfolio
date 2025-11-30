namespace MassImageEditor.Core;

/// <summary>
/// Stores configuration for image processing operations.
/// </summary>
public sealed class ProcessingSettings
{
    private static ProcessingSettings? _instance;

    public static ProcessingSettings Instance => _instance ??= new ProcessingSettings();

    public bool ResizeEnabled { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }

    public bool RotateEnabled { get; set; }
    public int RotationDegrees { get; set; }

    public bool ConvertEnabled { get; set; }
    public string? ConvertToFormat { get; set; }

    public bool BlackAndWhiteEnabled { get; set; }

    public int MaxThreads { get; set; } = 4;
    public bool BrightnessEnabled { get; set; }
    public int BrightnessValue { get; set; }
    public bool ContrastEnabled { get; set; }
    public int ContrastValue { get; set; }

    private ProcessingSettings()
    {
    }
}
