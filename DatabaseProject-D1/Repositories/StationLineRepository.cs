using Microsoft.Data.SqlClient;

namespace DatabazeProjekt.Repositories;

public sealed class StationLineRepository : IStationLineRepository
{
    private readonly SqlConnection _connection;

    public StationLineRepository(SqlConnection connection)
    {
        _connection = connection;
    }

    public void AddStationToLine(int stationId, int lineId)
    {
        AddStationToLine(stationId, lineId, null);
    }

    public void AddStationToLine(int stationId, int lineId, SqlTransaction? transaction)
    {
        const string query =
            "INSERT INTO stanice_linka (stanice_id, linka_id) VALUES (@StationId, @LineId)";

        using var cmd = new SqlCommand(query, _connection);
        if (transaction != null) cmd.Transaction = transaction;
        cmd.Parameters.AddWithValue("@StationId", stationId);
        cmd.Parameters.AddWithValue("@LineId", lineId);
        cmd.ExecuteNonQuery();
    }
}
