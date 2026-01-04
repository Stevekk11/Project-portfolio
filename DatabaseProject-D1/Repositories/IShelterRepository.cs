namespace DatabazeProjekt.Repositories;

public interface IShelterRepository
{
    void AddShelter(int stationId, string type, string color, string owner, string administrator, DateTime manufacturingDate);

    // Returns true if at least one shelter row was deleted.
    bool TryDeleteShelterByStationName(string stationName);
}
