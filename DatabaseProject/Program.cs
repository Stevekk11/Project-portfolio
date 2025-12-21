
using DatabazeProjekt;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            MessageBox.Show($"Unhandled App Exception: {(e.ExceptionObject as Exception)?.Message}", "Unhandled exception, uh oh", MessageBoxButtons.OK, MessageBoxIcon.Error);

        ApplicationConfiguration.Initialize();
        Application.Run(new MainWindow());

    }
}