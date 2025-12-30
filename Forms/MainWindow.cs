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
        ConnStr.CheckedChanged += ConnStr_CheckedChanged;
        WinAuth.CheckedChanged += ConnStr_CheckedChanged;
    }
    
    /// <summary>
    /// Kontroluje enabled/disabled stav polí při výběru typu připojení (checkboxy).
    /// </summary>
    private void ConnStr_CheckedChanged(object? sender, EventArgs e)
    {
        
        // - ConnStr: použije App.config connection string (uživatel/heslo/server/db se nesmí editovat)
        // - WinAuth: použije Windows auth (uživatel/heslo se nesmí editovat)
        // - žádný: SQL auth (uživatel/heslo + server/db)

        // pokud uživatel zapne ConnStr, vypneme WinAuth (jsou vzájemně vylučující)
        if (sender == ConnStr && ConnStr.Checked)
            WinAuth.Checked = false;

        // pokud uživatel zapne WinAuth, vypneme ConnStr
        if (sender == WinAuth && WinAuth.Checked)
            ConnStr.Checked = false;

        if (ConnStr.Checked)
        {
            UsernameField.Enabled = false;
            PasswordField.Enabled = false;
            ConfigName.Enabled = true;
            ServerField.Enabled = false;
            DBField.Enabled = false;
            return;
        }

        if (WinAuth.Checked)
        {
            UsernameField.Enabled = false;
            PasswordField.Enabled = false;
            ConfigName.Enabled = false;
            ServerField.Enabled = true;
            DBField.Enabled = true;
            return;
        }

        // SQL auth
        UsernameField.Enabled = true;
        PasswordField.Enabled = true;
        ConfigName.Enabled = false;
        ServerField.Enabled = true;
        DBField.Enabled = true;
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
            // Validace vstupů
            if (ConnStr.Checked)
            {
                string configName = ConfigName.Text;
                if (string.IsNullOrWhiteSpace(configName))
                {
                    ErrorField.Text = "Chyba: Zadejte název připojovacího řetězce z App.config";
                    return;
                }
                connectionString = ConfigurationManager.ConnectionStrings[configName]?.ConnectionString;
                if (connectionString == null)
                {
                    ErrorField.Text = $"Chyba: Připojovací řetězec '{configName}' nebyl nalezen v App.config";
                    return;
                }
            }
            else if (WinAuth.Checked)
            {
                if (String.IsNullOrWhiteSpace(server))
                {
                    ErrorField.Text = "Chyba: Zadejte server";
                    return;
                }
                if (String.IsNullOrWhiteSpace(database))
                {
                    ErrorField.Text = "Chyba: Zadejte databázi";
                    return;
                }
                connectionString = $"Server={server};Database={database};Integrated Security=true;TrustServerCertificate=true";
            }
            else
            {
                if (String.IsNullOrWhiteSpace(server))
                {
                    ErrorField.Text = "Chyba: Zadejte server";
                    return;
                }
                if (String.IsNullOrWhiteSpace(database))
                {
                    ErrorField.Text = "Chyba: Zadejte databázi";
                    return;
                }
                if (String.IsNullOrWhiteSpace(username))
                {
                    ErrorField.Text = "Chyba: Zadejte uživatelské jméno";
                    return;
                }
                if (String.IsNullOrWhiteSpace(password))
                {
                    ErrorField.Text = "Chyba: Zadejte heslo";
                    return;
                }
                connectionString = $"Server={server};Database={database};User Id={username};Password={password};TrustServerCertificate=true";
            }

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            
            ErrorField.Text = "✓ Připojení do databáze se zdařilo";
            ErrorField.ForeColor = System.Drawing.Color.DarkGreen;
            
            MessageBox.Show("Připojení do databáze se zdařilo.", "Úspěch", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            
            //opens the next window
            Chooser chooser = new Chooser(connection);
            chooser.Show();
        }
        catch (SqlException sqlEx)
        {
            string errorMsg = $"Chyba SQL: {sqlEx.Message}";
            ErrorField.Text = errorMsg;
            ErrorField.ForeColor = System.Drawing.Color.DarkRed;
            MessageBox.Show(errorMsg, "Chyba připojení", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            string errorMsg = $"Chyba: {ex.Message}";
            ErrorField.Text = errorMsg;
            ErrorField.ForeColor = System.Drawing.Color.DarkRed;
            MessageBox.Show(errorMsg, "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string cs = ConfigurationManager.ConnectionStrings["Doprava"]?.ConnectionString;
            if (cs == null)
            {
                ErrorField.Text = "Chyba: Připojovací řetězec 'Doprava' nebyl nalezen v App.config";
                ErrorField.ForeColor = Color.DarkRed;
                MessageBox.Show("Připojovací řetězec 'Doprava' nebyl nalezen v App.config", "Chyba konfigurace", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SqlConnection connection = new SqlConnection(cs);
            connection.Open();

            ErrorField.Text = "✓ Připojení k databázi Doprava se zdařilo";
            ErrorField.ForeColor = Color.DarkGreen;

            Transport doprava = new Transport(connection);
            doprava.Show();
        }
        catch (SqlException sqlEx)
        {
            string errorMsg = $"Chyba SQL: {sqlEx.Message}";
            ErrorField.Text = errorMsg;
            ErrorField.ForeColor = Color.DarkRed;
            MessageBox.Show(errorMsg, "Chyba připojení", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception exception)
        {
            string errorMsg = $"Chyba: {exception.Message}";
            ErrorField.Text = errorMsg;
            ErrorField.ForeColor = Color.DarkRed;
            MessageBox.Show(errorMsg, "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}