using System.Data;
using Microsoft.Data.SqlClient;

namespace DatabazeProjekt;

public partial class AddShelter : Form
{
    private SqlConnection connection;

    public AddShelter(SqlConnection conn)
    {
        InitializeComponent();
        this.connection = conn;
    }
/// <summary>
/// This method adds the shelter with the provided values.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
    private void odeslat_Click(object sender, EventArgs e)
    {
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            string query =
                "insert into pristresek (stanice_id,typ,barva, vlastnik, spravce,datum_vyroby) values (@staniceId,@typ,@barva,@vlastnik,@spravce,@datum_vyr)";
            cmd.CommandText = query;
            int staniceId = GetId();
            string typ = typPrist.Text;
            string barva = this.barva.Text;
            string vlastnik = this.vlastnik.Text;
            string spravce = this.spravce.Text;
            DateTime datumVyroby = datum.Value;
            cmd.Parameters.Add("@staniceId", SqlDbType.Int).Value = staniceId;
            cmd.Parameters.AddWithValue("@typ", typ);
            cmd.Parameters.AddWithValue("@barva", barva);
            cmd.Parameters.AddWithValue("@vlastnik", vlastnik);
            cmd.Parameters.AddWithValue("@spravce", spravce);
            cmd.Parameters.Add("@datum_vyr", SqlDbType.DateTime).Value = datumVyroby;
            cmd.ExecuteNonQuery();
            MessageBox.Show("Přístřešek byl úspěšně přidán.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cmd.Dispose();
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
        cmd.Parameters.AddWithValue("@staniceName", stanice.Text);
        return (int)cmd.ExecuteScalar();
    }
}