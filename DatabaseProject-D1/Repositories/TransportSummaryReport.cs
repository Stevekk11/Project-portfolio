namespace DatabazeProjekt.Repositories;

public sealed class TransportSummaryReport
{
    public required DateTime GeneratedAt { get; init; }

    // Table counts
    public int StationCount { get; init; }
    public int LineCount { get; init; }
    public int StationLineLinkCount { get; init; }
    public int ShelterCount { get; init; }
    public int MetroStationCount { get; init; }
    public int TrainStationCount { get; init; }

    // Stations by type
    public int BusStationCount { get; init; }
    public int TramStationCount { get; init; }
    public int MetroStationBaseCount { get; init; }
    public int TrainStationBaseCount { get; init; }

    // Line coverage (from v_linky_s_pokrytim)
    public int MinStationsPerLine { get; init; }
    public int MaxStationsPerLine { get; init; }
    public double AvgStationsPerLine { get; init; }

    // Metro depth stats (from v_linky_s_pokrytim)
    public double MinMetroDepth { get; init; }
    public double MaxMetroDepth { get; init; }

    // Train stations
    public int MinPlatformCount { get; init; }
    public int MaxPlatformCount { get; init; }
    public double AvgPlatformCount { get; init; }
}

