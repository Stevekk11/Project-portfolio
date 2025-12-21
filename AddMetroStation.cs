using Microsoft.Data.SqlClient;

namespace DatabazeProjekt;

public partial class AddMetroStation : Form
{
    private SqlConnection conn;

    public AddMetroStation(SqlConnection connection)
    {
        InitializeComponent();
        this.conn = connection;
    }
/// <summary>
/// Sends the data to the database
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
    private void odeslat_Click(object sender, EventArgs e)
    {
        try
        {
            SqlCommand command = conn.CreateCommand();
            int id = GetId();
            double hloubka = Convert.ToDouble(this.hloubka.Text);
            string uklid = this.uklid.Text;
            bool maWc = wc.Checked;
            DateTime datumUklidu = datPoslUkl.Value;
            command.CommandText =
                "insert into metro_stanice(stanice_id, hloubka_pod_zemi, cetnost_uklidu, ma_wc, dat_posl_uklid) values (@id,@hloubka,@uklid,@maWc,@datumUklidu)";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@hloubka", hloubka);
            command.Parameters.AddWithValue("@uklid", uklid);
            command.Parameters.AddWithValue("@maWc", maWc);
            command.Parameters.AddWithValue("@datumUklidu", datumUklidu);
            command.ExecuteNonQuery();
            command.Dispose();
            MessageBox.Show("Metro stanice byla úspěšně přidána.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        
    }
/// <summary>
/// Gets the station id by name.
/// </summary>
/// <returns></returns>
    private int GetId()
    {
        SqlCommand command = conn.CreateCommand();
        command.CommandText = "SELECT id_stanice FROM stanice where nazev = @nazev";
        command.Parameters.AddWithValue("@nazev", nazevStanice.Text);
        return (int) command.ExecuteScalar();
    }
}