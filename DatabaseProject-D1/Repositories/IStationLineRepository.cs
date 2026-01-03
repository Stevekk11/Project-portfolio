using Microsoft.Data.SqlClient;

namespace DatabazeProjekt.Repositories;

public interface IStationLineRepository
{
    void AddStationToLine(int stationId, int lineId);
    void AddStationToLine(int stationId, int lineId, SqlTransaction? transaction);
}
