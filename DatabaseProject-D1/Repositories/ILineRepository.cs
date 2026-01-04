using Microsoft.Data.SqlClient;

namespace DatabazeProjekt.Repositories;

public interface ILineRepository
{
    /// <summary>
    /// Gets existing line id by line number, or creates the line and returns new id.
    /// </summary>
    int GetOrCreateLineId(int lineNumber, string lineName);
    int GetOrCreateLineId(int lineNumber, string lineName, SqlTransaction? transaction);

    // Returns true if a row was deleted.
    bool TryDeleteLineByNumber(int lineNumber);
}
