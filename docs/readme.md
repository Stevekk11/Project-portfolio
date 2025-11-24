# MassImageEditor
## Autor: Štěpán Végh

A high-performance Windows Forms application for batch processing images with multiple filters and transformations using multi-threaded processing. School project

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)
![C#](https://img.shields.io/badge/C%23-12.0-239120)
![Platform](https://img.shields.io/badge/platform-Windows-blue)

## Features

- **Batch Image Processing**: Process hundreds or thousands of images simultaneously
- **Multi-threaded Performance**: Configurable worker threads for optimal CPU utilization
- **Multiple Image Filters**:
    -  Resize with aspect ratio preservation
    -  Rotate (90°, 180°, 270°)
    -  Black & White (Grayscale) conversion
    -  Format conversion (JPG, PNG, BMP, WebP)
- **Real-time Progress Tracking**: Visual feedback for each image's processing status
- **Cancellation Support**: Stop processing at any time
- **Professional UI**: Clean, intuitive Windows Forms interface

## 📋 Requirements

- **Operating System**: Windows 10/11 or later
- **.NET Runtime**: .NET 9.0 or later
- **Memory**: 4GB RAM minimum (8GB+ recommended for large batches)
- **Display**: 1280x720 minimum resolution

##  Installation

### Option 1: Download Release (Recommended)
1. Go to the [Releases](https://github.com/Stevekk11/MassImageEditor/releases) page
2. Download the latest `MassImageEditor.zip`
3. Extract and run `MassImageEditor.exe`

### Option 2: Build from Source
```bash
# Clone the repository
git clone https://github.com/Stevekk11/MassImageEditor.git
cd MassImageEditor

# Build the project
dotnet build --configuration Release

# Run the application
dotnet run --configuration Release
```

## Usage
### Basic Workflow

1. **Select Input Folder**: Click "Input Folder" and choose the folder containing your images
2. **Select Output Folder**: Click "Output Folder" and choose where processed images will be saved
3. **Configure Settings**: Click "Settings" to configure processing options
4. **Start Processing**: Click "Start" to begin batch processing
5. **Monitor Progress**: Watch the real-time progress bar and status updates

### Supported Image Formats

**Input**: JPG, JPEG, PNG, BMP, GIF, WebP  
**Output**: JPG, PNG, BMP, WebP (configurable)

### Configuration Options

#### Resize Settings
- **Enable Resize**: Toggle image resizing
- **Width**: Target width in pixels
- **Height**: Target height in pixels
- Maintains aspect ratio with letterboxing

#### Rotation Settings
- **Enable Rotate**: Toggle rotation
- **Degrees**: 90°, 180°, or 270°

#### Format Conversion
- **Enable Convert**: Toggle format conversion
- **Target Format**: Choose JPG, PNG, BMP, or WebP

#### Black & White Filter
- **Enable B&W**: Convert images to grayscale
- **Brightness**: Adjust brightness
## License
This project is a school project by Štěpán Végh (2025).

## Errors
Out of memory - out of RAM
Corrupted image - corrupted file
## PlantUML Diagram
![img.png](img.png)