using Microsoft.Data.SqlClient;

namespace DatabazeProjekt.Repositories;

public sealed class MetroStationRepository : IMetroStationRepository
{
    private readonly SqlConnection _connection;

    public MetroStationRepository(SqlConnection connection)
    {
        _connection = connection;
    }

    public void AddMetroStation(int stationId, double depthUnderGround, string cleaningFrequency, bool hasWc,
        DateTime lastCleaningDate)
    {
        const string query =
            "insert into metro_stanice(stanice_id, hloubka_pod_zemi, cetnost_uklidu, ma_wc, dat_posl_uklid) values (@id,@hloubka,@uklid,@maWc,@datumUklidu)";

        using var cmd = new SqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@id", stationId);
        cmd.Parameters.AddWithValue("@hloubka", depthUnderGround);
        cmd.Parameters.AddWithValue("@uklid", cleaningFrequency ?? string.Empty);
        cmd.Parameters.AddWithValue("@maWc", hasWc);
        cmd.Parameters.AddWithValue("@datumUklidu", lastCleaningDate);
        cmd.ExecuteNonQuery();
    }

    public bool TryDeleteMetroStationByStationName(string stationName)
    {
        const string query = @"
DELETE ms
FROM dbo.metro_stanice ms
INNER JOIN dbo.stanice s ON s.id_stanice = ms.stanice_id
WHERE s.nazev = @name";

        using var cmd = new SqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@name", stationName);
        return cmd.ExecuteNonQuery() > 0;
    }
}
