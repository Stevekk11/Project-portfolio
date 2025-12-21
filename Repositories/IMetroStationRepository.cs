namespace DatabazeProjekt.Repositories;

public interface IMetroStationRepository
{
    void AddMetroStation(int stationId, double depthUnderGround, string cleaningFrequency, bool hasWc, DateTime lastCleaningDate);
}

