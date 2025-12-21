using Microsoft.Data.SqlClient;

namespace DatabazeProjekt;

public partial class AddTrainStation : Form
{
    private SqlConnection connection;

    public AddTrainStation(SqlConnection conn)
    {
        InitializeComponent();
        this.connection = conn;
    }
/// <summary>
/// This method adds the train station with the given parameters.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
    private void Odeslat_Click(object sender, EventArgs e)
    {
        try
        {
            int staniceId = GetId();
            SqlCommand command = connection.CreateCommand();
            int pocetNast = Nastupiste.Value;
            int rozchod = Convert.ToInt32(Rozchod.Text);
            bool elekrifikovana = Elektrifikovana.Checked;
            var soustava = Soustava.Text;

            string query =
                "insert into vlak_stanice(stanice_id, pocet_nast, elektrifikovana, soustava, rozchod_kolej) values (@staniceId, @pocet,@elektrifikovana,@soustava,@rozchod)";
            command.Parameters.AddWithValue("@staniceId", staniceId);
            command.Parameters.AddWithValue("@pocet", pocetNast);
            command.Parameters.AddWithValue("@rozchod", rozchod);
            command.Parameters.AddWithValue("@elektrifikovana", elekrifikovana);
            command.Parameters.AddWithValue("@soustava", soustava);
            command.CommandText = query;
            command.ExecuteNonQuery();
            MessageBox.Show("Vlaková stanice byla úspěšně přidána.", "Úspěch", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            command.Dispose();
            this.Close();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    /// <summary>
    /// This method is used to retrieve the id of the given station.
    /// </summary>
    /// <returns>The ID</returns>
    private int GetId()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connection;
        cmd.CommandText = "select id_stanice from stanice where nazev = @staniceName";
        cmd.Parameters.AddWithValue("@staniceName", StaniceJmeno.Text);
        return (int)cmd.ExecuteScalar();
    }
}