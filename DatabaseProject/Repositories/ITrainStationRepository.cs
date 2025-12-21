namespace DatabazeProjekt.Repositories;

public interface ITrainStationRepository
{
    void AddTrainStation(int stationId, int platformCount, bool isElectrified, string electricalSystem, int trackGauge);
}

