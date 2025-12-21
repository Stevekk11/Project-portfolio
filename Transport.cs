using Microsoft.Data.SqlClient;

namespace DatabazeProjekt;
/// <summary>
/// Class for the Doprava DB
/// </summary>
public partial class Transport : Form
{
    private SqlConnection connection;

    public Transport(SqlConnection connection)
    {
        InitializeComponent();

        this.connection = connection;
    }

    /// <summary>
    /// Quits the app.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void exit_Click(object sender, EventArgs e)
    {
        connection.Close();
        this.Close();
    }

    /// <summary>
    /// Used for adding a station and a line manually
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void staniceLinka_Click(object sender, EventArgs e)
    {
        AddStationAndLine station = new AddStationAndLine(connection);
        station.ShowDialog();
    }

    /// <summary>
    ///  Used for adding a shelter manually
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void pristresek_Click(object sender, EventArgs e)
    {
        AddShelter shelter = new AddShelter(connection);
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
        AddMetroStation metro = new AddMetroStation(connection);
        metro.ShowDialog();
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
        var config =
            new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture);
        //You can change the delimiter depending on the CSV file.
        switch (Delimiter.Checked)
        {
            case true:
                config.Delimiter = ";";
                break;
            case false:
                config.Delimiter = ",";
                break;
        }

        using (var reader = new StreamReader(filePath, System.Text.Encoding.UTF8)) //important to use for Czech!
        using (var csvData = new CsvHelper.CsvReader(reader, config))
        {
            // Step 1: Read all records from the CSV file
            var records = csvData.GetRecords<StationRecord>().ToList();


            config.HeaderValidated = null;

            foreach (var record in records)
            {
                // Step 2: Insert into 'linky' table
                string insertLinkyQuery = @"
                        IF NOT EXISTS (SELECT 1 FROM dbo.linky WHERE cislo_linky = @LineNumber)
                        BEGIN
                            INSERT INTO dbo.linky (cislo_linky, nazev_linky)
                            VALUES (@LineNumber, @LineName)
                        END";

                using (SqlCommand cmdLinky = new SqlCommand(insertLinkyQuery, connection))
                {
                    cmdLinky.Parameters.AddWithValue("@LineNumber", record.LineNumber);
                    cmdLinky.Parameters.AddWithValue("@LineName", record.LineName); // Assuming no name is provided for the line
                    cmdLinky.ExecuteNonQuery();
                }

                // Step 3: Insert into 'stanice' table
                string insertStaniceQuery = @"
                        INSERT INTO dbo.stanice (nazev, typ_stanice, ma_pristresek, ma_lavicku, ma_kos, ma_infopanel, na_znameni, bezbarierova)
                        VALUES (@StationName, @StationType, @HasShelter, @HasBench, @HasTrashBin, @HasInfoPanel, @RequestStop, @BarrierFree);
                        SELECT SCOPE_IDENTITY();";

                int stationId;
                using (SqlCommand cmdStanice = new SqlCommand(insertStaniceQuery, connection))
                {
                    cmdStanice.Parameters.AddWithValue("@StationName", record.StationName);
                    cmdStanice.Parameters.AddWithValue("@StationType", record.StationType);
                    cmdStanice.Parameters.AddWithValue("@HasShelter", record.HasShelter);
                    cmdStanice.Parameters.AddWithValue("@HasBench", record.HasBench);
                    cmdStanice.Parameters.AddWithValue("@HasTrashBin", record.HasTrashBin);
                    cmdStanice.Parameters.AddWithValue("@HasInfoPanel", record.HasInfoPanel);
                    cmdStanice.Parameters.AddWithValue("@RequestStop", record.RequestStop);
                    cmdStanice.Parameters.AddWithValue("@BarrierFree", record.BarrierFree);

                    // Retrieve the newly inserted station ID
                    stationId = Convert.ToInt32(cmdStanice.ExecuteScalar());
                }

                // Step 4: Link 'stanice' and 'linky' in 'stanice_linka' table
                string insertStaniceLinkaQuery = @"
                        INSERT INTO dbo.stanice_linka (stanice_id, linka_id)
                        VALUES (@StationId, (SELECT id_linky FROM dbo.linky WHERE cislo_linky = @LineNumber))";

                using (SqlCommand cmdStaniceLinka = new SqlCommand(insertStaniceLinkaQuery, connection))
                {
                    cmdStaniceLinka.Parameters.AddWithValue("@StationId", stationId);
                    cmdStaniceLinka.Parameters.AddWithValue("@LineNumber", record.LineNumber);
                    cmdStaniceLinka.ExecuteNonQuery();
                }
            }
        }

        MessageBox.Show("Data byla vložena.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void vlak_Click(object sender, EventArgs e)
    {
        AddTrainStation pv = new AddTrainStation(connection);
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
        SqlCommand command = connection.CreateCommand();

        try
        {
            if (SmazStanici.Checked)
            {
                string name = SmazStaniciJmeno.Text;

                command.CommandText = "select id_stanice from stanice where nazev = @name";
                command.Parameters.AddWithValue("@name", name);
                int id = Convert.ToInt32(command.ExecuteScalar());
                command.CommandText = "delete from stanice_linka where stanice_id = @id_stanice";
                command.Parameters.AddWithValue("@id_stanice", id);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                command.CommandText = "delete from stanice where id_stanice = @id_stanice";
                command.Parameters.AddWithValue("@id_stanice", id);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }

            if (SmazLinku.Checked)
            {
                int cislo = Convert.ToInt32(SmazLinkuCislo.Text);

                command.Parameters.AddWithValue("@cislo", cislo);
                command.CommandText = "delete from linky where cislo_linky = @cislo";
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }

            if (SmazPrist.Checked)
            {
                command.CommandText = "select id_stanice from stanice where nazev = @name";
                string name = SmazPristJmeno.Text;
                command.Parameters.AddWithValue("@name", name);
                int id = Convert.ToInt32(command.ExecuteScalar());
                command.CommandText = "delete from pristresek where stanice_id = @id_stanice";
                command.Parameters.AddWithValue("@id_stanice", id);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }

            if (SmazMetro.Checked)
            {
                command.CommandText = "select id_stanice from stanice where nazev = @name";
                string name = SmazMetroJmeno.Text;
                command.Parameters.AddWithValue("@name", name);
                int id = Convert.ToInt32(command.ExecuteScalar());
                command.CommandText = "delete from metro_stanice where stanice_id = @id_stanice";
                command.Parameters.AddWithValue("@id_stanice", id);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }

            if (SmazVlak.Checked)
            {
                command.CommandText = "select id_stanice from stanice where nazev = @name";
                string name = SmazVlakJmeno.Text;
                command.Parameters.AddWithValue("@name", name);
                int id = Convert.ToInt32(command.ExecuteScalar());
                command.CommandText = "delete from vlak_stanice where stanice_id = @id_stanice";
                command.Parameters.AddWithValue("@id_stanice", id);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }

            command.Dispose();
            MessageBox.Show("Data úspěšně smazána.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}