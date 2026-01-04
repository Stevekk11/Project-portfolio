﻿namespace DatabazeProjekt.Repositories;

public interface ITrainStationRepository
{
    void AddTrainStation(int stationId, int platformCount, bool isElectrified, string electricalSystem, int trackGauge);

    // Returns true if a matching train-station row was deleted.
    bool TryDeleteTrainStationByStationName(string stationName);
}
