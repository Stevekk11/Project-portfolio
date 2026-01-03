using Microsoft.Data.SqlClient;
using DatabazeProjekt.Repositories;
using DatabazeProjekt.Reports;

namespace DatabazeProjekt;
/// <summary>
/// Class for the Doprava DB
/// </summary>
public partial class Transport : Form
{
    private SqlConnection _connection;
    private IStationRepository _stationRepository;
    private IReportRepository _reportRepository;

    public Transport(SqlConnection connection)
    {
        InitializeComponent();
        this._connection = connection;
        this._stationRepository = new StationRepository(connection);
        this._reportRepository = new ReportRepository(connection);
    }

    /// <summary>
    /// Quits the app.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void exit_Click(object sender, EventArgs e)
    {
        _connection.Close();
        this.Close();
    }

    /// <summary>
    /// Used for adding a station and a line manually
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void staniceLinka_Click(object sender, EventArgs e)
    {
        AddStationAndLine station = new AddStationAndLine(_connection);
        station.ShowDialog();
    }

    /// <summary>
    ///  Used for adding a shelter manually
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void pristresek_Click(object sender, EventArgs e)
    {
        AddShelter shelter = new AddShelter(_connection);
        shelter.ShowDialog();
    }

    /// <summary>
    /// Used for adding a metro station manually.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void metro_Click(object sender, EventArgs e)
    {
        AddMetroStation metroForm = new AddMetroStation(_connection);
        metroForm.ShowDialog();
    }

    /// <summary>
    /// This method is used for opening a CSV file.
    /// Parts of this method were made by AI.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void csv_Click(object sender, EventArgs e)
    {
        try
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.Title = "Select a CSV File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    // Read and display CSV data
                    InsertDataIntoDatabase(filePath);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// Here, we will insert all the data from the csv into the Stanice and Linky tables.
    /// </summary>
    /// <param name="filePath">File path obtained from the dialog window</param>
    private void InsertDataIntoDatabase(string filePath)
    {
        var config = new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture);
        switch (Delimiter.Checked)
        {
            case true:
                config.Delimiter = ";";
                break;
            case false:
                config.Delimiter = ",";
                break;
        }

        using (var reader = new StreamReader(filePath, System.Text.Encoding.UTF8))
        using (var csvData = new CsvHelper.CsvReader(reader, config))
        {
            var records = csvData.GetRecords<StationRecord>().ToList();
            config.HeaderValidated = null;
            foreach (var record in records)
            {
                // Use repository for station insert
                _stationRepository.AddStation(record);
                // ...existing code for linking lines, etc. (can be refactored into repositories as well)...
            }
        }
        MessageBox.Show("Data byla vložena.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void vlak_Click(object sender, EventArgs e)
    {
        AddTrainStation pv = new AddTrainStation(_connection);
        pv.ShowDialog();
    }

    /// <summary>
    /// This method is used to change the CSV delimiter. Default ","
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Delimiter_CheckedChanged(object sender, EventArgs e)
    {
        MessageBox.Show("CSV oddělovač se změnil", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void Smazat_Click(object sender, EventArgs e)
    {

        try
        {
            if (SmazStanici.Checked)
            {
                string name = SmazStaniciJmeno.Text;
                _stationRepository.DeleteStationByName(name);
            }
            MessageBox.Show("Data úspěšně smazána.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void Report_Click(object sender, EventArgs e)
    {
        try
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "Markdown (*.md)|*.md|All files (*.*)|*.*",
                Title = "Uložit souhrnný report",
                FileName = $"doprava-report-{DateTime.Now:yyyyMMdd-HHmm}.md",
                AddExtension = true,
                DefaultExt = "md"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            var report = _reportRepository.GetTransportSummaryReport();
            var md = MarkdownReportWriter.ToMarkdown(report);
            File.WriteAllText(sfd.FileName, md, System.Text.Encoding.UTF8);

            MessageBox.Show($"Report uložen do:\n{sfd.FileName}", "Hotovo", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Nepodařilo se vygenerovat report: {ex.Message}", "Chyba", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void EditorBtn_Click(object sender, EventArgs e)
    {
        Chooser chooser = new Chooser(_connection);
        chooser.Show();
    }
}