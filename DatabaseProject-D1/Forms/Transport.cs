using Microsoft.Data.SqlClient;
using DatabazeProjekt.Repositories;
using DatabazeProjekt.Reports;

namespace DatabazeProjekt.Forms;

public partial class Transport : Form
{
    private SqlConnection _connection;
    private IStationRepository _stationRepository;
    private IReportRepository _reportRepository;

    private ILineRepository _lineRepository;
    private IStationLineRepository _stationLineRepository;
    private IShelterRepository _shelterRepository;
    private IMetroStationRepository _metroStationRepository;
    private ITrainStationRepository _trainStationRepository;

    private readonly CheckBox[] _deleteOptions;

    public Transport(SqlConnection connection)
    {
        InitializeComponent();
        this._connection = connection;
        this._stationRepository = new StationRepository(connection);
        this._reportRepository = new ReportRepository(connection);

        _lineRepository = new LineRepository(connection);
        _stationLineRepository = new StationLineRepository(connection);
        _shelterRepository = new ShelterRepository(connection);
        _metroStationRepository = new MetroStationRepository(connection);
        _trainStationRepository = new TrainStationRepository(connection);

        _deleteOptions = new[] { SmazStanici, SmazLinku, SmazPrist, SmazMetro, SmazVlak };

        foreach (var cb in _deleteOptions)
            cb.CheckedChanged += DeleteOption_CheckedChanged;

        UpdateDeleteInputs();

        EditorBtn.Click += EditorBtn_Click;
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
            config.HeaderValidated = null;
            var records = csvData.GetRecords<StationRecord>().ToList();

            var insertedRows = 0;

            foreach (var record in records)
            {

                record.StationName = (record.StationName ?? string.Empty).Trim();
                record.StationType = (record.StationType ?? string.Empty).Trim();
                record.LineName = (record.LineName ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(record.StationName))
                    throw new InvalidOperationException("Import CSV: Pole StationName je povinné.");

                if (string.IsNullOrWhiteSpace(record.StationType))
                    throw new InvalidOperationException($"Import CSV: Pro stanici je vyžadován StationType. '{record.StationName}'.");

                if (record.LineNumber <= 0)
                    throw new InvalidOperationException($"Import CSV: Číslo linky musí být kladné celé číslo pro stanici '{record.StationName}'.");

                using (var transaction = _connection.BeginTransaction())
                {
                    try
                    {

                        var stationId = _stationRepository.AddStationAndReturnId(record, transaction);
                        var lineId = _lineRepository.GetOrCreateLineId(record.LineNumber, record.LineName, transaction);
                        _stationLineRepository.AddStationToLine(stationId, lineId, transaction);

                        transaction.Commit();
                        insertedRows++;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            MessageBox.Show($"Data byla vložena. Importováno řádků: {insertedRows}.", "Info", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
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
            var selected = _deleteOptions.Where(x => x.Checked).ToList();
            if (selected.Count == 0)
            {
                MessageBox.Show("Vyberte, co chcete smazat.", "Smazání", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (selected.Count > 1)
            {
                MessageBox.Show("Vyberte prosím pouze jednu možnost smazání.", "Smazání", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            bool deleted;
            string entity;
            string identifier;

            if (SmazStanici.Checked)
            {
                entity = "stanici";
                identifier = (SmazStaniciJmeno.Text ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    MessageBox.Show("Zadejte název stanice.", "Smazání", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    SmazStaniciJmeno.Focus();
                    return;
                }

                if (!ConfirmDelete(entity, identifier, extraWarning:
                        "Smazáním stanice se smažou i navázané záznamy (metro/vlak/přístřešky/přiřazení linek)."))
                    return;

                deleted = _stationRepository.TryDeleteStationByName(identifier);
            }
            else if (SmazLinku.Checked)
            {
                entity = "linku";
                identifier = (SmazLinkuCislo.Text ?? string.Empty).Trim();
                if (!int.TryParse(identifier, out var lineNumber) || lineNumber <= 0)
                {
                    MessageBox.Show("Zadejte platné číslo linky (kladné celé číslo).", "Smazání",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SmazLinkuCislo.Focus();
                    return;
                }

                if (!ConfirmDelete(entity, lineNumber.ToString(),
                        extraWarning: "Smazáním linky se smažou i její přiřazení ke stanicím."))
                    return;

                deleted = _lineRepository.TryDeleteLineByNumber(lineNumber);
                identifier = lineNumber.ToString();
            }
            else if (SmazPrist.Checked)
            {
                entity = "přístřešek";
                identifier = (SmazPristJmeno.Text ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    MessageBox.Show("Zadejte název stanice, u které chcete smazat přístřešek.", "Smazání",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SmazPristJmeno.Focus();
                    return;
                }

                if (!ConfirmDelete(entity, identifier,
                        extraWarning: "Smažou se všechny přístřešky, které jsou evidované u této stanice."))
                    return;

                deleted = _shelterRepository.TryDeleteShelterByStationName(identifier);
            }
            else if (SmazMetro.Checked)
            {
                entity = "metro stanici";
                identifier = (SmazMetroJmeno.Text ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    MessageBox.Show("Zadejte název stanice, u které chcete smazat metro údaje.", "Smazání",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SmazMetroJmeno.Focus();
                    return;
                }

                if (!ConfirmDelete(entity, identifier))
                    return;

                deleted = _metroStationRepository.TryDeleteMetroStationByStationName(identifier);
            }
            else if (SmazVlak.Checked)
            {
                entity = "vlakovou stanici";
                identifier = (SmazVlakJmeno.Text ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    MessageBox.Show("Zadejte název stanice, u které chcete smazat vlakové údaje.", "Smazání",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SmazVlakJmeno.Focus();
                    return;
                }

                if (!ConfirmDelete(entity, identifier))
                    return;

                deleted = _trainStationRepository.TryDeleteTrainStationByStationName(identifier);
            }
            else
            {
                // Should never happen because we validated exactly one checkbox is checked.
                return;
            }

            if (deleted)
            {
                MessageBox.Show($"Položka byla úspěšně smazána ({entity}: {identifier}).", "Hotovo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearDeleteInputs();
            }
            else
            {
                MessageBox.Show($"Nic jsem nesmazal: {entity} '{identifier}' nebyla nalezena.", "Nenalezeno",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DeleteOption_CheckedChanged(object? sender, EventArgs e)
    {
        if (sender is not CheckBox changed)
            return;

        // Make it behave like a radio-choice (one option at a time), but keep CheckBox controls.
        if (changed.Checked)
        {
            foreach (var cb in _deleteOptions)
            {
                if (!ReferenceEquals(cb, changed))
                    cb.Checked = false;
            }
        }

        UpdateDeleteInputs();
    }

    private void UpdateDeleteInputs()
    {
        SmazStaniciJmeno.Enabled = SmazStanici.Checked;
        SmazLinkuCislo.Enabled = SmazLinku.Checked;
        SmazPristJmeno.Enabled = SmazPrist.Checked;
        SmazMetroJmeno.Enabled = SmazMetro.Checked;
        SmazVlakJmeno.Enabled = SmazVlak.Checked;

        // Disable delete button until something is selected.
        Smazat.Enabled = _deleteOptions.Any(x => x.Checked);
    }

    private void ClearDeleteInputs()
    {
        SmazStaniciJmeno.Text = string.Empty;
        SmazLinkuCislo.Text = string.Empty;
        SmazPristJmeno.Text = string.Empty;
        SmazMetroJmeno.Text = string.Empty;
        SmazVlakJmeno.Text = string.Empty;
    }

    private bool ConfirmDelete(string entity, string identifier, string? extraWarning = null)
    {
        var message = $"Opravdu chcete smazat {entity} '{identifier}'?";
        if (!string.IsNullOrWhiteSpace(extraWarning))
            message += $"\n\nPozor: {extraWarning}";

        return MessageBox.Show(message, "Potvrzení smazání", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                   MessageBoxDefaultButton.Button2) == DialogResult.Yes;
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

