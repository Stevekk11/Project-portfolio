using System.Configuration;
using Serilog;

namespace MassImageEditor;
/// <summary>
/// Copyright (c) 2025 Štěpán Végh. Licensed under the MIT License.
/// </summary>
static class Program
{
    /// <summary>
    /// Entry point for the MassImageEditor application.
    /// Configures logging, handles unhandled exceptions, and starts the main application window.
    /// </summary>
    [STAThread]
    static void Main()
    {
        string? logFolder = ConfigurationManager.AppSettings["LogFolder"];
        //default log folder to "Logs" if not specified in config
        if (string.IsNullOrWhiteSpace(logFolder))
           logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        Directory.CreateDirectory(logFolder);
        var logPath = Path.Combine(logFolder, "MassImageEditor--.log");
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
                path: logPath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                rollOnFileSizeLimit: true,
                shared: true)
            .CreateLogger();

        try
        {
            Log.Information("Application starting up");

            ApplicationConfiguration.Initialize();

            Application.ThreadException += (sender, args) =>
            {
                Log.Error(args.Exception, "Unhandled UI thread exception");
                MessageBox.Show("An unexpected error occurred. See log for details.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var ex = args.ExceptionObject as Exception;
                Log.Fatal(ex, "Unhandled non‑UI thread exception");
            };

            Application.Run(new MainWindow());
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}

