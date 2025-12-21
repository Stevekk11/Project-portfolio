using DatabazeProjekt.Repositories;
using Microsoft.Data.SqlClient;

namespace DatabazeProjekt;

public partial class AddTrainStation : Form
{
    private readonly SqlConnection connection;
    private readonly IStationRepository _stationRepository;
    private readonly ITrainStationRepository _trainStationRepository;

    public AddTrainStation(SqlConnection conn)
    {
        InitializeComponent();
        this.connection = conn;
        _stationRepository = new StationRepository(conn);
        _trainStationRepository = new TrainStationRepository(conn);
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
            int pocetNast = Nastupiste.Value;
            int rozchod = Convert.ToInt32(Rozchod.Text);
            bool elekrifikovana = Elektrifikovana.Checked;
            var soustava = Soustava.Text;

            _trainStationRepository.AddTrainStation(staniceId, pocetNast, elekrifikovana, soustava, rozchod);

            MessageBox.Show("Vlaková stanice byla úspěšně přidána.", "Úspěch", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
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
        return _stationRepository.GetStationIdByName(StaniceJmeno.Text);
    }
}