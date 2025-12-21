using Microsoft.Data.SqlClient;

namespace DatabazeProjekt;

public partial class AddStationAndLine : Form
{
    private SqlConnection connection;

    public AddStationAndLine(SqlConnection conn)
    {
        InitializeComponent();
        this.connection = conn;
    }
/// <summary>
/// This method adds the station and the line going through it.
/// Parts of this method were made by AI.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
    private void odeslat_Click(object sender, EventArgs e)
    {
        try
        {
            string stationName = nazevStanice.Text;
            string stationType = typStanice.Text;
            bool hasShelter = prist.Checked;
            bool hasBench = lavice.Checked;
            bool hasTrashBin = kos.Checked;
            bool hasInfoPanel = infop.Checked;
            bool requestStop = naznam.Checked;
            bool barrierFree = bezba.Checked;
            int lineNumber = Convert.ToInt32(cisloLinky.Text);
            string lineName = nazevLinky.Text;

            string staniceQuery = @"
                INSERT INTO stanice (ma_lavicku, ma_kos, ma_pristresek, ma_infopanel, na_znameni, bezbarierova, typ_stanice, nazev)
                OUTPUT INSERTED.id_stanice
                VALUES (@HasBench, @HasTrashBin, @HasShelter, @HasInfoPanel, @OnDemand, @BarrierFree, @StationType, @StationName)";
            int staniceId;
            //made by AI
            using (SqlCommand cmd = new SqlCommand(staniceQuery, connection))
            {
                cmd.Parameters.AddWithValue("@HasBench", hasBench);
                cmd.Parameters.AddWithValue("@HasTrashBin", hasTrashBin);
                cmd.Parameters.AddWithValue("@HasShelter", hasShelter);
                cmd.Parameters.AddWithValue("@HasInfoPanel", hasInfoPanel);
                cmd.Parameters.AddWithValue("@OnDemand", requestStop);
                cmd.Parameters.AddWithValue("@BarrierFree", barrierFree);
                cmd.Parameters.AddWithValue("@StationType", stationType);
                cmd.Parameters.AddWithValue("@StationName", stationName);

                staniceId = (int)cmd.ExecuteScalar();
            }
            
            string linkaQuery = @"
                IF NOT EXISTS (SELECT 1 FROM linky WHERE cislo_linky = @LineNumber)
                BEGIN
                    INSERT INTO linky (cislo_linky, nazev_linky) OUTPUT INSERTED.id_linky VALUES (@LineNumber, @nazevLinky)
                END
                ELSE
                BEGIN
                    SELECT id_linky FROM linky WHERE cislo_linky = @LineNumber
                END";
            int lineId;
            using (SqlCommand cmd2 = new SqlCommand(linkaQuery, connection))
            {
                cmd2.Parameters.AddWithValue("@LineNumber", lineNumber);
                cmd2.Parameters.AddWithValue("@nazevLinky", lineName);

                lineId = (int)cmd2.ExecuteScalar();
            }

            string insertStationLineQuery = @"
                INSERT INTO stanice_linka (stanice_id, linka_id)
                VALUES (@StationId, @LineId)";
            using (SqlCommand cmd = new SqlCommand(insertStationLineQuery, connection))
            {
                cmd.Parameters.AddWithValue("@StationId", staniceId);
                cmd.Parameters.AddWithValue("@LineId", lineId);

                cmd.ExecuteNonQuery();
            }
            //end of AI
            MessageBox.Show("Stanice i linka byly přidány.","Úspěch",MessageBoxButtons.OK,MessageBoxIcon.Information);
            this.Close();
        }
        catch (Exception exception)
        {
            MessageBox.Show($"An error occurred: {exception.Message}","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
    }
}