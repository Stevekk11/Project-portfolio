﻿using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DatabazeProjekt.Repositories
{
    public interface IStationRepository
    {
        void AddStation(StationRecord station);
        void DeleteStationByName(string name);

        // Returns true if a row was deleted, false if nothing matched.
        bool TryDeleteStationByName(string name);

        StationRecord GetStationByName(string name);
        IEnumerable<StationRecord> GetAllStations();
        int AddStationAndReturnId(StationRecord station);
        int AddStationAndReturnId(StationRecord station, SqlTransaction? transaction);
        int GetStationIdByName(string name);
    }
}
