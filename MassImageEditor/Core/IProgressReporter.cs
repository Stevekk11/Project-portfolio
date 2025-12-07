namespace MassImageEditor.Core;

public interface IProgressReporter
{
    void ReportProgress(int percent, string message = null);
}