using System.Data;
using Microsoft.Data.SqlClient;

namespace DatabazeProjekt;
/// <summary>
/// Database editor
/// </summary>
public partial class Chooser : Form
{
    private SqlConnection connection;

    public Chooser(SqlConnection openConnection)
    {
        InitializeComponent();
        connection = openConnection;
        LoadTables();
    }
/// <summary>
/// Loads all the names of the tables from the DB.
/// </summary>
    private void LoadTables()
    {
        try
        {
            SqlCommand command = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES", connection);
            SqlDataReader reader = command.ExecuteReader();

            TableSelectBox.Items.Clear();
            while (reader.Read())
            {
                TableSelectBox.Items.Add(reader["TABLE_NAME"].ToString());
            }

            reader.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Chyba načítání tabulek: {ex.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

/// <summary>
/// Helper method for convrting the names to string.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
    private void TableSelectBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadTableData(TableSelectBox.SelectedItem.ToString());
    }
/// <summary>
/// Loads the table data into the app.
/// </summary>
/// <param name="tableName"></param>
    private void LoadTableData(string tableName)
    {
        try
        {
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM {tableName}", connection);
            DataTable tableData = new DataTable();
            adapter.Fill(tableData);

            dataGridView1.DataSource = tableData;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Chyba načítání tabulek: {ex.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
/// <summary>
/// Method for adding a row to a given table.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
    private void Add_Click(object sender, EventArgs e)
    {
        DataTable table = (DataTable)dataGridView1.DataSource;
        if (table != null)
        {
            DataRow newRow = table.NewRow();
            table.Rows.Add(newRow);
        }
    }
/// <summary>
/// Method for deleting a row from a given table.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
    private void Delete_Click(object sender, EventArgs e)
    {
        foreach (DataGridViewRow row in dataGridView1.SelectedRows)
        {
            if (!row.IsNewRow)
            {
                dataGridView1.Rows.Remove(row);
            }
        }
    }
/// <summary>
/// Method for saving the changes made to the table.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
    private void Save_Click(object sender, EventArgs e)
    {
        {
            string selectedTable = TableSelectBox.SelectedItem.ToString();

            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM {selectedTable}", connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                DataTable changes = ((DataTable)dataGridView1.DataSource).GetChanges();
                if (changes != null)
                {
                    adapter.Update(changes);
                    ((DataTable)dataGridView1.DataSource).AcceptChanges();
                    MessageBox.Show("Data uložena.", "Success", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba ukládání dat: {ex.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
/// <summary>
/// Method used for importing data from a csv file.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
    private void Import_Click(object sender, EventArgs e)
    {
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.Title = "Select a CSV File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                // Read and display CSV data
                DataTable dataTable = ReadCsvFile(filePath);
                dataGridView1.DataSource = dataTable;
            }
        }
    }
/// <summary>
/// Reads the csv file.
/// </summary>
/// <param name="filePath">File path of the csv</param>
/// <returns></returns>
    private DataTable ReadCsvFile(string filePath)
    {
        DataTable dt = new DataTable();

        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string[] headers = sr.ReadLine().Split(';');

                // Add columns to DataTable
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                // Add rows to DataTable
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(';');
                    dt.Rows.Add(rows);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error reading CSV file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        return dt;
    }
    /// <summary>
    /// Exits the window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Exit_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}