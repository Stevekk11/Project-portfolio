using System.Configuration;
using Microsoft.Data.SqlClient;

namespace DatabazeProjekt;

/// <summary>
/// The main window for the UI
/// </summary>
public partial class MainWindow : Form
{
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    ///  Loads the given database from the text fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LoadDB_Click(object sender, EventArgs e)
    {
        string connectionString = null;
        string server = ServerField.Text;
        string database = DBField.Text;
        string username = UsernameField.Text;
        string password = PasswordField.Text;

        try
        {
            //No app.config used here for the string due to the nature of the user entering the details.
            if (WinAuth.Checked && !String.IsNullOrWhiteSpace(server) && !String.IsNullOrWhiteSpace(database))
            {
                connectionString =
                    $"Server={server};Database={database};Integrated Security = true;TrustServerCertificate=true";
            }
            else
            {
                connectionString =
                    $"Server={server};Database={database};User Id={username};Password={password};TrustServerCertificate=true";
            }

            if (ConnStr.Checked)
            {
                connectionString = ConfigurationManager.ConnectionStrings[$"{ConfigName.Text}"].ConnectionString;
            }


            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();
            MessageBox.Show("Připojení do databáze se zdařilo.", "Success", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            //opens the next window
            Chooser chooser = new Chooser(connection);
            chooser.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Připojení do databáze se nezdařilo... Error: {ex.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            ErrorField.Text = ex.Message;
            Console.WriteLine(ex);
        }
    }

    /// <summary>
    /// Quits the app.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ExitDB_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    /// <summary>
    /// Opens the database doprava in the application for further editing.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Doprava_Click(object sender, EventArgs e)
    {
        try
        {
            string cs = ConfigurationManager.ConnectionStrings["Doprava"].ConnectionString;
            SqlConnection connection = new SqlConnection(cs);
            connection.Open();

            Transport doprava = new Transport(connection);
            doprava.Show();
        }
        catch (Exception exception)
        {
            ErrorField.Text = exception.Message;
        }
    }
}