using Serilog;

namespace MassImageEditor;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
                path: "../../../Logs/MassImageEditor-.log",
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

