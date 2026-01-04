﻿namespace DatabazeProjekt.Repositories;

public interface IMetroStationRepository
{
    void AddMetroStation(int stationId, double depthUnderGround, string cleaningFrequency, bool hasWc, DateTime lastCleaningDate);

    // Returns true if a matching metro row was deleted.
    bool TryDeleteMetroStationByStationName(string stationName);
}
