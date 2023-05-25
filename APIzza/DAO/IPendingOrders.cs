using APIzza.DTO;
using APIzza.Models;

namespace APIzza.DAO
{
    public interface IPendingOrders
    {
        IList<PendingOrders> ViewPendingOrders();
        IList<PendingOrders> ViewHistoricalReport(int employeeId);
        IList<PendingOrders> GetPizzaById(int id);
        IList<SalesDto> SalesReport();
    }
}
