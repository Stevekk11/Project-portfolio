using System.Data;
using DatabazeProjekt.Repositories;
using Microsoft.Data.SqlClient;

namespace DatabazeProjekt;

public partial class AddShelter : Form
{
    private readonly SqlConnection connection;
    private readonly IStationRepository _stationRepository;
    private readonly IShelterRepository _shelterRepository;

    public AddShelter(SqlConnection conn)
    {
        InitializeComponent();
        this.connection = conn;
        _stationRepository = new StationRepository(conn);
        _shelterRepository = new ShelterRepository(conn);
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
            int staniceId = GetId();
            string typ = typPrist.Text;
            string barva = this.barva.Text;
            string vlastnik = this.vlastnik.Text;
            string spravce = this.spravce.Text;
            DateTime datumVyroby = datum.Value;

            _shelterRepository.AddShelter(staniceId, typ, barva, vlastnik, spravce, datumVyroby);

            MessageBox.Show("Přístřešek byl úspěšně přidán.", "Úspěch", MessageBoxButtons.OK,
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
        return _stationRepository.GetStationIdByName(stanice.Text);
    }
}