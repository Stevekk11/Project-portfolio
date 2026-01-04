using Microsoft.Data.SqlClient;

namespace DatabazeProjekt.Repositories;

public sealed class LineRepository : ILineRepository
{
    private readonly SqlConnection _connection;

    public LineRepository(SqlConnection connection)
    {
        _connection = connection;
    }

    public int GetOrCreateLineId(int lineNumber, string lineName)
    {
        return GetOrCreateLineId(lineNumber, lineName, null);
    }

    public int GetOrCreateLineId(int lineNumber, string lineName, SqlTransaction? transaction)
    {
        const string query = @"
            IF NOT EXISTS (SELECT 1 FROM linky WHERE cislo_linky = @LineNumber)
            BEGIN
                INSERT INTO linky (cislo_linky, nazev_linky)
                OUTPUT INSERTED.id_linky
                VALUES (@LineNumber, @LineName)
            END
            ELSE
            BEGIN
                SELECT id_linky FROM linky WHERE cislo_linky = @LineNumber
            END";

        using var cmd = new SqlCommand(query, _connection);
        if (transaction != null) cmd.Transaction = transaction;
        cmd.Parameters.AddWithValue("@LineNumber", lineNumber);
        cmd.Parameters.AddWithValue("@LineName", lineName ?? string.Empty);

        return (int)cmd.ExecuteScalar();
    }

    public bool TryDeleteLineByNumber(int lineNumber)
    {
        const string query = "DELETE FROM dbo.linky WHERE cislo_linky = @LineNumber";
        using var cmd = new SqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@LineNumber", lineNumber);
        return cmd.ExecuteNonQuery() > 0;
    }
}
