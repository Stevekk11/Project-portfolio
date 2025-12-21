namespace DatabazeProjekt;
/// <summary>
/// This class is used to identify the CSV parameters.
/// </summary>
public class StationRecord
{
    public string StationName { get; set; }
    public string StationType { get; set; }
    public bool HasShelter { get; set; }
    public bool HasBench { get; set; }
    public bool HasTrashBin { get; set; }
    public bool HasInfoPanel { get; set; }
    public bool RequestStop { get; set; }
    public bool BarrierFree { get; set; }
    public int LineNumber { get; set; }
    public string LineName { get; set; }
}