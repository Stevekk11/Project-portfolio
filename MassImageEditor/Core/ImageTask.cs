namespace MassImageEditor.Core;

/// <summary>
/// Represents a task for processing an image file.
/// This class encapsulates the file path for the input image and the path where the output will be saved.
/// </summary>
public sealed class ImageTask
{
    public string FilePath { get; set; }
    public string OutputPath { get; set; }

    public ImageTask(string filePath, string outputPath)
    {
        FilePath = filePath;
        OutputPath = outputPath;
    }
}
//Copyright 2025