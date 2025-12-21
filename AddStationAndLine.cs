using DatabazeProjekt.Repositories;
using Microsoft.Data.SqlClient;

namespace DatabazeProjekt;

public partial class AddStationAndLine : Form
{
    private readonly SqlConnection connection;
    private readonly IStationRepository _stationRepository;
    private readonly ILineRepository _lineRepository;
    private readonly IStationLineRepository _stationLineRepository;

    public AddStationAndLine(SqlConnection conn)
    {
        InitializeComponent();
        this.connection = conn;
        _stationRepository = new StationRepository(conn);
        _lineRepository = new LineRepository(conn);
        _stationLineRepository = new StationLineRepository(conn);
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

            var station = new StationRecord
            {
                StationName = stationName,
                StationType = stationType,
                HasShelter = hasShelter,
                HasBench = hasBench,
                HasTrashBin = hasTrashBin,
                HasInfoPanel = hasInfoPanel,
                RequestStop = requestStop,
                BarrierFree = barrierFree
            };

            int stationId = _stationRepository.AddStationAndReturnId(station);
            int lineId = _lineRepository.GetOrCreateLineId(lineNumber, lineName);
            _stationLineRepository.AddStationToLine(stationId, lineId);

            MessageBox.Show("Stanice i linka byly přidány.", "Úspěch", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            this.Close();
        }
        catch (Exception exception)
        {
            MessageBox.Show($"An error occurred: {exception.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}