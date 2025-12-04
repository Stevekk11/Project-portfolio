# MassImageEditor
## Autor: Štěpán Végh, datum: Prosinec 2025
## Škola: SPŠE Ječná, Praha 2

A high-performance Windows Forms application for batch processing images with multiple filters and transformations using multi-threaded processing. School project.

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)
![C#](https://img.shields.io/badge/C%23-12.0-239120)
![Platform](https://img.shields.io/badge/platform-Windows-blue)

## Table of Contents
- [User Requirements](#user-requirements)
- [Features](#features)
- [Requirements](#-requirements)
- [Installation](#installation)
- [Usage](#usage)
- [Architecture](#architecture)
- [Runtime Behavior](#runtime-behavior)
- [Dependencies and Third-Party Libraries](#dependencies-and-third-party-libraries)
- [Configuration](#configuration)
- [Error Handling](#error-handling)
- [Testing and Validation](#testing-and-validation)
- [Known Issues](#known-issues)
- [License](#license)

## User Requirements

### Functional Requirements
1. **FR-01**: The application shall allow users to select an input folder containing images
2. **FR-02**: The application shall allow users to select an output folder for processed images
3. **FR-03**: The application shall support batch processing of multiple images simultaneously
4. **FR-04**: The application shall support image resizing with configurable width and height
5. **FR-05**: The application shall support image rotation (90°, 180°, 270°)
6. **FR-06**: The application shall support black and white (grayscale) conversion
7. **FR-07**: The application shall support brightness adjustment (-100 to +100)
8. **FR-08**: The application shall support contrast adjustment (-100 to +100)
9. **FR-09**: The application shall support sharpness adjustment (0 to 100)
10. **FR-10**: The application shall support format conversion (JPG, PNG, BMP, WebP)
11. **FR-11**: The application shall display real-time progress for each image
12. **FR-12**: The application shall allow cancellation of processing at any time
13. **FR-13**: The application shall support configurable number of worker threads
14. **FR-14**: The application shall preserve original images (non-destructive processing)

### Non-Functional Requirements
1. **NFR-01**: Performance - Process at least 100 images per minute on modern hardware
2. **NFR-02**: Scalability - Support processing batches of 1000+ images
3. **NFR-03**: Usability - Intuitive UI requiring minimal training
4. **NFR-04**: Reliability - Handle errors gracefully without crashing
5. **NFR-05**: Resource Management - Efficient memory usage with proper disposal of image objects
6. **NFR-06**: Thread Safety - Safe concurrent access to shared resources

### Use Cases
See [docs_class-diagram.puml](docs_class-diagram.puml) for detailed UML Use Case and Sequence diagrams.

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

#### Brightness Adjustment
- **Enable Brightness**: Toggle brightness adjustment
- **Brightness Value**: Range from -100 (darker) to +100 (brighter)

#### Contrast Adjustment
- **Enable Contrast**: Toggle contrast adjustment
- **Contrast Value**: Range from -100 (less contrast) to +100 (more contrast)

#### Sharpness Adjustment
- **Enable Sharpness**: Toggle sharpness enhancement
- **Sharpness Value**: Range from 0 (no sharpening) to 100 (maximum sharpening), default: 25

#### Performance Settings
- **Max Threads**: Number of worker threads (1-16), default: 4
  - Higher values = faster processing but more CPU/memory usage
  - Recommended: Number of CPU cores or slightly higher

## Architecture

### Design Patterns
The application uses several design patterns:

1. **Singleton Pattern**: `ProcessingSettings` class ensures single configuration instance
2. **Pipeline Pattern**: `ImageProcessorPipeline` chains multiple processors sequentially
3. **Producer-Consumer Pattern**: `ImageTaskQueue` with `BlockingCollection<T>` for thread-safe task distribution
4. **Observer Pattern**: Event-based communication between workers and UI
5. **Strategy Pattern**: `IImageProcessor` interface with multiple implementations

### Component Descriptions

- **MainWindow**: Primary UI, manages user interactions and coordinates processing
- **Settings**: Configuration dialog for processing parameters
- **ImagePreviewForm**: Displays image previews
- **ProcessingSettings**: Singleton holding all configuration values
- **ImageTask**: Encapsulates input/output file paths for a single image
- **ImageTaskQueue**: Thread-safe queue using `BlockingCollection<T>`
- **ImageWorker**: Worker thread that processes tasks from the queue
- **ImageProcessorPipeline**: Chains multiple processors and applies them sequentially
- **IImageProcessor**: Interface for all image processing operations
- **FormatSaver**: Handles image format conversion and saving

### Class Diagram
See [docs_class-diagram.puml](class_diagram.puml) for detailed UML Class diagram.

## Runtime Behavior

### Image Processing Workflow

1. **User selects folders** → MainWindow stores input/output paths
2. **User configures settings** → Settings modifies ProcessingSettings singleton
3. **User clicks Start** → MainWindow creates ImageTaskQueue and ImageWorker instances
4. **MainWindow scans input folder** → Creates ImageTask for each file and enqueues
5. **ImageWorkers start** (multiple threads) →
   - Dequeue ImageTask
   - Load image from FilePath
   - Create ImageProcessorPipeline from settings
   - Apply processors sequentially:
     - Resize (if enabled)
     - Rotate (if enabled)
     - Black & White (if enabled)
     - Brightness (if enabled)
     - Contrast (if enabled)
     - Sharpness (if enabled)
   - FormatSaver saves to OutputPath with correct format
   - Fire TaskCompleted event
6. **MainWindow updates UI** → Updates ListView item status and progress
7. **Repeat** until all tasks complete or user cancels

### State Transitions
- **Idle** → User selects folders → **Ready**
- **Ready** → User clicks Start → **Processing**
- **Processing** → All tasks complete → **Completed**
- **Processing** → User clicks Cancel → **Cancelled**
- **Completed/Cancelled** → User clicks Clear → **Idle**

### Sequence Diagram
See [docs_class-diagram.puml](behavioral_diagram.puml) for detailed UML Sequence diagram showing the complete processing flow.

## Dependencies and Third-Party Libraries

### NuGet Packages

| Package | Version | Purpose | License |
|---------|---------|---------|---------|
| **Serilog** | 4.2.0 | Structured logging framework | Apache 2.0 |
| **Serilog.Sinks.Console** | 6.0.0 | Console output for logs | Apache 2.0 |
| **Serilog.Sinks.File** | 6.0.0 | File-based logging | Apache 2.0 |
| **NUnit** | 4.2.2 | Unit testing framework | MIT |
| **NUnit.Analyzers** | 4.5.0 | Code analyzers for NUnit | MIT |
| **NUnit3TestAdapter** | 4.6.0 | Test adapter for Visual Studio | MIT |
| **Microsoft.NET.Test.Sdk** | 17.12.0 | Testing platform | MIT |

### .NET Framework Components
- **System.Drawing**: Core image manipulation (Bitmap, Graphics)
- **System.Windows.Forms**: UI framework
- **System.Collections.Concurrent**: Thread-safe collections (BlockingCollection)
- **System.Threading.Tasks**: Async/await and Task parallel library

### External Dependencies
- **Windows GDI+**: System.Drawing relies on Windows Graphics Device Interface
- **Windows Forms Runtime**: Requires Windows OS with .NET Desktop Runtime

## Configuration

### Application Configuration File
Location: `Configuration/App.config`

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <appSettings>
    <!-- Logging settings -->
    <add key="LogLevel" value="Information"/>
    <add key="LogPath" value="Logs/app.log"/>
    
    <!-- Performance settings -->
    <add key="DefaultThreadCount" value="4"/>
    <add key="QueueCapacity" value="100"/>
  </appSettings>
</configuration>
```

### Settings Persistence
- Settings are stored in `ProcessingSettings` singleton during runtime
- No persistent storage between sessions (resets on restart)
- Future enhancement: Save/load settings from JSON or XML file

### Configuration Parameters

| Parameter | Type | Default | Range | Description |
|-----------|------|---------|-------|-------------|
| ResizeEnabled | bool | false | - | Enable/disable resizing |
| Width | int? | null | 1-10000 | Target width in pixels |
| Height | int? | null | 1-10000 | Target height in pixels |
| RotateEnabled | bool | false | - | Enable/disable rotation |
| RotationDegrees | int | 0 | 0,90,180,270 | Rotation angle |
| ConvertEnabled | bool | false | - | Enable/disable format conversion |
| ConvertToFormat | string? | null | JPG,PNG,BMP,WEBP | Target format |
| BlackAndWhiteEnabled | bool | false | - | Enable/disable B&W conversion |
| BrightnessEnabled | bool | false | - | Enable/disable brightness adjustment |
| BrightnessValue | int | 0 | -100 to +100 | Brightness adjustment value |
| ContrastEnabled | bool | false | - | Enable/disable contrast adjustment |
| ContrastValue | int | 0 | -100 to +100 | Contrast adjustment value |
| SharpnessEnabled | bool | false | - | Enable/disable sharpness |
| SharpnessValue | int | 25 | 0-100 | Sharpness intensity |
| MaxThreads | int | 4 | 1-16 | Number of worker threads |

## Error Handling

### Common Errors and Resolutions

#### Error: "No input folder selected"
- **Cause**: User clicked Start without selecting input folder
- **Resolution**: Click "Input Folder" button and select a folder containing images
- **Prevention**: Start button validates folder selection

#### Error: "No output folder selected"
- **Cause**: User clicked Start without selecting output folder
- **Resolution**: Click "Output Folder" button and select destination folder
- **Prevention**: Start button validates folder selection

#### Error: "No images found in selected folder"
- **Cause**: Selected input folder contains no supported image files
- **Resolution**: Verify folder contains JPG, PNG, BMP, GIF, or WebP files
- **Prevention**: Check folder contents before processing

#### Error: "Access denied to file"
- **Cause**: File is locked by another application or insufficient permissions
- **Resolution**: 
  - Close applications using the file
  - Run as Administrator if necessary
  - Check file permissions
- **Status**: Task marked as failed, processing continues with other images

#### Error: "Out of memory"
- **Cause**: Processing very large images or too many concurrent threads
- **Resolution**: 
  - Reduce MaxThreads setting
  - Process smaller batches
  - Close other memory-intensive applications
- **Prevention**: Monitor Task Manager during processing

#### Error: "Invalid image format"
- **Cause**: Corrupted image file or unsupported format
- **Resolution**: Verify file integrity, try opening in image viewer
- **Status**: Task marked as failed, processing continues

#### Error: "Disk full"
- **Cause**: Insufficient space in output folder
- **Resolution**: Free up disk space or select different output location
- **Prevention**: Check available space before processing large batches

### Logging
All errors are logged to:
- **Console**: Real-time output during development
- **File**: `Logs/app.log` for debugging and auditing
- **Format**: Structured logging with timestamp, level, and context

Example log entry:
```
2025-12-04 10:30:45.123 [ERR] Image processing failed for "image.jpg": System.IO.IOException: The process cannot access the file
```

## Testing and Validation

### Unit Tests
Location: `MIETests/` directory

#### Running Tests
```bash
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "FullyQualifiedName~BrightnessTest"

# Run with detailed output
dotnet test --verbosity detailed
```

## Known Issues

### Current Version Limitations


1. **No Undo Functionality**
   - **Issue**: Cannot undo batch processing operations
   - **Workaround**: Always keep backups of original images
   - **Planned Fix**: Not planned (by design - non-destructive processing)
   - **Priority**: N/A

2. **Limited Preview**
   - **Issue**: ImagePreviewForm is basic, no before/after comparison
   - **Workaround**: Open images in external viewer
   - **Planned Fix**: v2.0 - Enhanced preview with side-by-side comparison
   - **Priority**: Low

### Reporting Issues
Please report bugs or feature requests to: [GitHub Issues](https://github.com/Stevekk11/MassImageEditor/issues)

## License
This project is a school project by Štěpán Végh (2025).

### Copyright Notice
© 2025 Štěpán Végh. Published under the MIT License.

This software is created as an educational project for SPŠE Ječná, Praha 2.

### Third-Party Licenses
This project uses third-party libraries with the following licenses:
- **Serilog**: Apache License 2.0
- **NUnit**: MIT License
- **.NET Runtime**: MIT License

See individual package documentation for complete license texts.

