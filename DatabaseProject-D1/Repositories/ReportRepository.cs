using Microsoft.Data.SqlClient;

namespace DatabazeProjekt.Repositories;

public sealed class ReportRepository : IReportRepository
{
    private readonly SqlConnection _connection;

    public ReportRepository(SqlConnection connection)
    {
        _connection = connection;
    }

    public TransportSummaryReport GetTransportSummaryReport()
    {
        // Aggregates across at least 3 tables:
        // - stanice
        // - linky
        // - stanice_linka
        // plus: pristresek, metro_stanice, vlak_stanice, and view v_linky_s_pokrytim
        const string sql = @"
SELECT
    -- counts
    (SELECT COUNT(*) FROM dbo.stanice) AS StationCount,
    (SELECT COUNT(*) FROM dbo.linky) AS LineCount,
    (SELECT COUNT(*) FROM dbo.stanice_linka) AS StationLineLinkCount,
    (SELECT COUNT(*) FROM dbo.pristresek) AS ShelterCount,
    (SELECT COUNT(*) FROM dbo.metro_stanice) AS MetroStationCount,
    (SELECT COUNT(*) FROM dbo.vlak_stanice) AS TrainStationCount,

    -- stations by type
    (SELECT COUNT(*) FROM dbo.stanice WHERE typ_stanice = 'bus') AS BusStationCount,
    (SELECT COUNT(*) FROM dbo.stanice WHERE typ_stanice = 'tram') AS TramStationCount,
    (SELECT COUNT(*) FROM dbo.stanice WHERE typ_stanice = 'metro') AS MetroStationBaseCount,
    (SELECT COUNT(*) FROM dbo.stanice WHERE typ_stanice = 'vlak') AS TrainStationBaseCount,

    -- line coverage stats
    COALESCE((SELECT MIN(pocet_stanic) FROM dbo.v_linky_s_pokrytim), 0) AS MinStationsPerLine,
    COALESCE((SELECT MAX(pocet_stanic) FROM dbo.v_linky_s_pokrytim), 0) AS MaxStationsPerLine,
    COALESCE((SELECT AVG(CAST(pocet_stanic AS float)) FROM dbo.v_linky_s_pokrytim), 0) AS AvgStationsPerLine,

    -- metro depth stats across lines (view uses metro_stanice)
    COALESCE((SELECT MIN(NULLIF(min_hloubka_metra, 0)) FROM dbo.v_linky_s_pokrytim), 0) AS MinMetroDepth,
    COALESCE((SELECT MAX(max_hloubka_metra) FROM dbo.v_linky_s_pokrytim), 0) AS MaxMetroDepth,

    -- platform count (vlak_stanice)
    COALESCE((SELECT MIN(pocet_nast) FROM dbo.vlak_stanice), 0) AS MinBoardings,
    COALESCE((SELECT MAX(pocet_nast) FROM dbo.vlak_stanice), 0) AS MaxBoardings,
    COALESCE((SELECT AVG(CAST(pocet_nast AS float)) FROM dbo.vlak_stanice), 0) AS AvgBoardings;
";

        using var cmd = new SqlCommand(sql, _connection);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read())
            throw new InvalidOperationException("Failed to generate report (no result row). ");

        return new TransportSummaryReport
        {
            GeneratedAt = DateTime.Now,
            StationCount = reader.GetInt32(reader.GetOrdinal("StationCount")),
            LineCount = reader.GetInt32(reader.GetOrdinal("LineCount")),
            StationLineLinkCount = reader.GetInt32(reader.GetOrdinal("StationLineLinkCount")),
            ShelterCount = reader.GetInt32(reader.GetOrdinal("ShelterCount")),
            MetroStationCount = reader.GetInt32(reader.GetOrdinal("MetroStationCount")),
            TrainStationCount = reader.GetInt32(reader.GetOrdinal("TrainStationCount")),

            BusStationCount = reader.GetInt32(reader.GetOrdinal("BusStationCount")),
            TramStationCount = reader.GetInt32(reader.GetOrdinal("TramStationCount")),
            MetroStationBaseCount = reader.GetInt32(reader.GetOrdinal("MetroStationBaseCount")),
            TrainStationBaseCount = reader.GetInt32(reader.GetOrdinal("TrainStationBaseCount")),

            MinStationsPerLine = reader.GetInt32(reader.GetOrdinal("MinStationsPerLine")),
            MaxStationsPerLine = reader.GetInt32(reader.GetOrdinal("MaxStationsPerLine")),
            AvgStationsPerLine = reader.GetDouble(reader.GetOrdinal("AvgStationsPerLine")),

            MinMetroDepth = reader.GetDouble(reader.GetOrdinal("MinMetroDepth")),
            MaxMetroDepth = reader.GetDouble(reader.GetOrdinal("MaxMetroDepth")),

            MinPlatformCount = reader.GetInt32(reader.GetOrdinal("MinBoardings")),
            MaxPlatformCount = reader.GetInt32(reader.GetOrdinal("MaxBoardings")),
            AvgPlatformCount = reader.GetDouble(reader.GetOrdinal("AvgBoardings"))
        };
    }
}

