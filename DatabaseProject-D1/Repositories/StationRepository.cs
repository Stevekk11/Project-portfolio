using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DatabazeProjekt.Repositories
{
    public class StationRepository : IStationRepository
    {
        private readonly SqlConnection _connection;

        public StationRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public void AddStation(StationRecord station)
        {
            // Reuse the id-returning insert to keep behavior consistent.
            AddStationAndReturnId(station);
        }

        public int AddStationAndReturnId(StationRecord station)
        {
            return AddStationAndReturnId(station, null);
        }

        public int AddStationAndReturnId(StationRecord station, SqlTransaction? transaction)
        {
            const string insertQuery = "INSERT INTO dbo.stanice (nazev, typ_stanice, ma_pristresek, ma_lavicku, ma_kos, ma_infopanel, na_znameni, bezbarierova) " +
                                       "OUTPUT INSERTED.id_stanice " +
                                       "VALUES (@stationname, @stationtype, @hasshelter, @hasbench, @hastrashbin, @hasinfopanel, @requeststop, @barrierfree);";
            using (SqlCommand cmd = new SqlCommand(insertQuery, _connection))
            {
                if (transaction != null) cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@stationname", station.StationName);
                cmd.Parameters.AddWithValue("@stationtype", station.StationType);
                cmd.Parameters.AddWithValue("@hasshelter", station.HasShelter);
                cmd.Parameters.AddWithValue("@hasbench", station.HasBench);
                cmd.Parameters.AddWithValue("@hastrashbin", station.HasTrashBin);
                cmd.Parameters.AddWithValue("@hasinfopanel", station.HasInfoPanel);
                cmd.Parameters.AddWithValue("@requeststop", station.RequestStop);
                cmd.Parameters.AddWithValue("@barrierfree", station.BarrierFree);
                return (int)cmd.ExecuteScalar();
            }
        }

        public void DeleteStationByName(string name)
        {
            string deleteQuery = "DELETE FROM dbo.stanice WHERE nazev = @name";
            using (SqlCommand cmd = new SqlCommand(deleteQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.ExecuteNonQuery();
            }
        }

        public bool TryDeleteStationByName(string name)
        {
            const string deleteQuery = "DELETE FROM dbo.stanice WHERE nazev = @name";
            using var cmd = new SqlCommand(deleteQuery, _connection);
            cmd.Parameters.AddWithValue("@name", name);
            return cmd.ExecuteNonQuery() > 0;
        }

        public StationRecord GetStationByName(string name)
        {
            string selectQuery = "SELECT * FROM dbo.stanice WHERE nazev = @name";
            using (SqlCommand cmd = new SqlCommand(selectQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@name", name);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new StationRecord
                        {
                            StationName = reader["nazev"]?.ToString() ?? string.Empty,
                            StationType = reader["typ_stanice"]?.ToString() ?? string.Empty,
                            HasShelter = (bool)reader["ma_pristresek"],
                            HasBench = (bool)reader["ma_lavicku"],
                            HasTrashBin = (bool)reader["ma_kos"],
                            HasInfoPanel = (bool)reader["ma_infopanel"],
                            RequestStop = (bool)reader["na_znameni"],
                            BarrierFree = (bool)reader["bezbarierova"],
                        };
                    }
                }
            }
            return null;
        }

        public int GetStationIdByName(string name)
        {
            const string selectQuery = "SELECT id_stanice FROM dbo.stanice WHERE nazev = @name";
            using (SqlCommand cmd = new SqlCommand(selectQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@name", name);
                object? result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    throw new InvalidOperationException($"Station with name '{name}' was not found.");
                return (int)result;
            }
        }

        public IEnumerable<StationRecord> GetAllStations()
        {
            var stations = new List<StationRecord>();
            string selectQuery = "SELECT * FROM dbo.stanice";
            using (SqlCommand cmd = new SqlCommand(selectQuery, _connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    stations.Add(new StationRecord
                    {
                        StationName = reader["nazev"]?.ToString() ?? string.Empty,
                        StationType = reader["typ_stanice"]?.ToString() ?? string.Empty,
                        HasShelter = (bool)reader["ma_pristresek"],
                        HasBench = (bool)reader["ma_lavicku"],
                        HasTrashBin = (bool)reader["ma_kos"],
                        HasInfoPanel = (bool)reader["ma_infopanel"],
                        RequestStop = (bool)reader["na_znameni"],
                        BarrierFree = (bool)reader["bezbarierova"],
                    });
                }
            }
            return stations;
        }
    }
}
