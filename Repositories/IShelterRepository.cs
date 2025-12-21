namespace DatabazeProjekt.Repositories;

public interface IShelterRepository
{
    void AddShelter(int stationId, string type, string color, string owner, string administrator, DateTime manufacturingDate);
}

