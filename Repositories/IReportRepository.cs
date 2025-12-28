using Microsoft.Data.SqlClient;

namespace DatabazeProjekt.Repositories;

public interface IReportRepository
{
    TransportSummaryReport GetTransportSummaryReport();
}
