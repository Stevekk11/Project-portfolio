using System.Globalization;
using System.Text;
using DatabazeProjekt.Repositories;

namespace DatabazeProjekt.Reports;

public static class MarkdownReportWriter
{
    public static string ToMarkdown(TransportSummaryReport r)
    {
        var sb = new StringBuilder();

        sb.AppendLine("# Souhrnný report – Doprava");
        sb.AppendLine();
        sb.AppendLine($"Vygenerováno: `{r.GeneratedAt:yyyy-MM-dd HH:mm:ss}`");
        sb.AppendLine();

        sb.AppendLine("## Počty záznamů (agregace přes více tabulek)");
        sb.AppendLine();
        sb.AppendLine("| Tabulka | Počet |");
        sb.AppendLine("|---|---:|");
        sb.AppendLine($"| `stanice` | {r.StationCount} |");
        sb.AppendLine($"| `linky` | {r.LineCount} |");
        sb.AppendLine($"| `stanice_linka` | {r.StationLineLinkCount} |");
        sb.AppendLine($"| `pristresek` | {r.ShelterCount} |");
        sb.AppendLine($"| `metro_stanice` | {r.MetroStationCount} |");
        sb.AppendLine($"| `vlak_stanice` | {r.TrainStationCount} |");
        sb.AppendLine();

        sb.AppendLine("## Stanice podle typu");
        sb.AppendLine();
        sb.AppendLine("| Typ stanice | Počet |");
        sb.AppendLine("|---|---:|");
        sb.AppendLine($"| bus | {r.BusStationCount} |");
        sb.AppendLine($"| tram | {r.TramStationCount} |");
        sb.AppendLine($"| metro | {r.MetroStationBaseCount} |");
        sb.AppendLine($"| vlak | {r.TrainStationBaseCount} |");
        sb.AppendLine();

        sb.AppendLine("## Linky – pokrytí stanicemi (min/max/průměr)");
        sb.AppendLine();
        sb.AppendLine("Data: `v_linky_s_pokrytim` (odvozeno z `linky`, `stanice_linka`, `stanice`, `metro_stanice`) ");
        sb.AppendLine();
        sb.AppendLine("| Metrika | Hodnota |");
        sb.AppendLine("|---|---:|");
        sb.AppendLine($"| Minimum stanic na lince | {r.MinStationsPerLine} |");
        sb.AppendLine($"| Maximum stanic na lince | {r.MaxStationsPerLine} |");
        sb.AppendLine($"| Průměr stanic na lince | {r.AvgStationsPerLine.ToString("0.##", CultureInfo.InvariantCulture)} |");
        sb.AppendLine();

        sb.AppendLine("## Metro – hloubka (min/max)");
        sb.AppendLine();
        sb.AppendLine("| Metrika | Hodnota |");
        sb.AppendLine("|---|---:|");
        sb.AppendLine($"| Minimální hloubka metra (m) | {r.MinMetroDepth.ToString("0.##", CultureInfo.InvariantCulture)} |");
        sb.AppendLine($"| Maximální hloubka metra (m) | {r.MaxMetroDepth.ToString("0.##", CultureInfo.InvariantCulture)} |");
        sb.AppendLine();

        sb.AppendLine("## Vlakové stanice – počet nástupišť (min/max/průměr)");
        sb.AppendLine();
        sb.AppendLine("| Metrika | Hodnota |");
        sb.AppendLine("|---|---:|");
        sb.AppendLine($"| Minimum | {r.MinPlatformCount} |");
        sb.AppendLine($"| Maximum | {r.MaxPlatformCount} |");
        sb.AppendLine($"| Průměr  | {r.AvgPlatformCount.ToString("0.##", CultureInfo.InvariantCulture)} |");
        sb.AppendLine();

        sb.AppendLine("---");
        sb.AppendLine("Report generovala aplikace DatabazeProjekt.");

        return sb.ToString();
    }
}

