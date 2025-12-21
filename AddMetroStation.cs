using DatabazeProjekt.Repositories;
using Microsoft.Data.SqlClient;

namespace DatabazeProjekt;

public partial class AddMetroStation : Form
{
    private readonly SqlConnection conn;
    private readonly IStationRepository _stationRepository;
    private readonly IMetroStationRepository _metroStationRepository;

    public AddMetroStation(SqlConnection connection)
    {
        InitializeComponent();
        this.conn = connection;
        _stationRepository = new StationRepository(connection);
        _metroStationRepository = new MetroStationRepository(connection);
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
            int id = GetId();
            double hloubka = Convert.ToDouble(this.hloubka.Text);
            string uklid = this.uklid.Text;
            bool maWc = wc.Checked;
            DateTime datumUklidu = datPoslUkl.Value;

            _metroStationRepository.AddMetroStation(id, hloubka, uklid, maWc, datumUklidu);

            MessageBox.Show("Metro stanice byla úspěšně přidána.", "Úspěch", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
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
        return _stationRepository.GetStationIdByName(nazevStanice.Text);
    }
}