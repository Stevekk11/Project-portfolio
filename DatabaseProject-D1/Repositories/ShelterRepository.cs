﻿using System.Data;
using Microsoft.Data.SqlClient;

namespace DatabazeProjekt.Repositories;

public sealed class ShelterRepository : IShelterRepository
{
    private readonly SqlConnection _connection;

    public ShelterRepository(SqlConnection connection)
    {
        _connection = connection;
    }

    public void AddShelter(int stationId, string type, string color, string owner, string administrator,
        DateTime manufacturingDate)
    {
        const string query =
            "insert into pristresek (stanice_id,typ,barva, vlastnik, spravce,datum_vyroby) values (@staniceId,@typ,@barva,@vlastnik,@spravce,@datum_vyr)";

        using var cmd = new SqlCommand(query, _connection);
        cmd.Parameters.Add("@staniceId", SqlDbType.Int).Value = stationId;
        cmd.Parameters.AddWithValue("@typ", type ?? string.Empty);
        cmd.Parameters.AddWithValue("@barva", color ?? string.Empty);
        cmd.Parameters.AddWithValue("@vlastnik", owner ?? string.Empty);
        cmd.Parameters.AddWithValue("@spravce", administrator ?? string.Empty);
        cmd.Parameters.Add("@datum_vyr", SqlDbType.DateTime).Value = manufacturingDate;
        cmd.ExecuteNonQuery();
    }

    public bool TryDeleteShelterByStationName(string stationName)
    {
        const string query = @"
DELETE p
FROM dbo.pristresek p
INNER JOIN dbo.stanice s ON s.id_stanice = p.stanice_id
WHERE s.nazev = @name";

        using var cmd = new SqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@name", stationName);
        return cmd.ExecuteNonQuery() > 0;
    }
}
