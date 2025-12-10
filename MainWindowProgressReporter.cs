using MassImageEditor.Core;
using System.Windows.Forms;

namespace MassImageEditor
{
    public class MainWindowProgressReporter : IProgressReporter
    {
        private readonly ListViewItem _item;
        private readonly MainWindow _window;

        public MainWindowProgressReporter(MainWindow window, ListViewItem item)
        {
            _window = window;
            _item = item;
        }

        public void ReportProgress(int percent, string? message = null)
        {
            if (_window.InvokeRequired)
            {
                _window.BeginInvoke(new Action<int, string?>(ReportProgress), percent, message);
                return;
            }
            _item.SubItems[2].Text = $"{percent}%";
            _item.SubItems[3].Text = message ?? "";
        }
    }
}

