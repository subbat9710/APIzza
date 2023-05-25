using APIzza.DAO;
using APIzza.DTO;
using APIzza.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIzza.Controllers
{
    [Route("api/reports")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private IPendingOrders pendingOrders;
        private IUserDao userDao;

        public ReportsController(IPendingOrders _pendingOrders, IUserDao _userDao)
        {
            this.pendingOrders = _pendingOrders;
            this.userDao = _userDao;
        }

        [HttpGet("historical")]  //Employee can see the Historical Reports
        public ActionResult<IList<PendingOrders>> GetHistoricalReports()
        {
            string username = User.Identity.Name;
            int employeeId = userDao.GetUser(username).UserId;
            return Ok(pendingOrders.ViewHistoricalReport(employeeId));
        }

        [HttpGet("sales")] //Employee can see the Sales Reports
        public ActionResult<IList<SalesDto>> GetSalesReports(string orderType = "All")
        {
            var salesReport = pendingOrders.SalesReport();

            if (orderType != "All")
            {
                salesReport = salesReport.Where(order => order.OrderType.ToLower() == orderType.ToLower()).ToList();
            }

            return Ok(salesReport);
        }

    }
}
