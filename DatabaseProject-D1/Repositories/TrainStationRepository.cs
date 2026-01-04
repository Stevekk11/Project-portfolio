using Microsoft.Data.SqlClient;

namespace DatabazeProjekt.Repositories;

public sealed class TrainStationRepository : ITrainStationRepository
{
    private readonly SqlConnection _connection;

    public TrainStationRepository(SqlConnection connection)
    {
        _connection = connection;
    }

    public void AddTrainStation(int stationId, int platformCount, bool isElectrified, string electricalSystem,
        int trackGauge)
    {
        const string query =
            "insert into vlak_stanice(stanice_id, pocet_nast, elektrifikovana, soustava, rozchod_kolej) " +
            "values (@staniceId, @pocet,@elektrifikovana,@soustava,@rozchod)";

        using var cmd = new SqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@staniceId", stationId);
        cmd.Parameters.AddWithValue("@pocet", platformCount);
        cmd.Parameters.AddWithValue("@rozchod", trackGauge);
        cmd.Parameters.AddWithValue("@elektrifikovana", isElectrified);
        cmd.Parameters.AddWithValue("@soustava", electricalSystem ?? string.Empty);
        cmd.ExecuteNonQuery();
    }

    public bool TryDeleteTrainStationByStationName(string stationName)
    {
        const string query = @"
DELETE vs
FROM dbo.vlak_stanice vs
INNER JOIN dbo.stanice s ON s.id_stanice = vs.stanice_id
WHERE s.nazev = @name";

        using var cmd = new SqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@name", stationName);
        return cmd.ExecuteNonQuery() > 0;
    }
}
