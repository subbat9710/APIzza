using APIzza.DAO;
using APIzza.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIzza.Controllers
{
    [Route("employee/api/order")]
    [ApiController]
    public class OrderController : Controller
    {
        public readonly IOrderDAO orderDAO;
        public readonly IUserDao userDao;


        public OrderController(IOrderDAO _orderDAO, IUserDao _userDao)
        {
            this.orderDAO = _orderDAO;
            this.userDao = _userDao;
        }


        [HttpPut("{id}/status")] //Employee can able to change the status of the orders
        public ActionResult<bool> UpdateOrderStatus(int id, string updateStatus)
        {
            EmailConfirmation email = new EmailConfirmation(orderDAO);

            string username = User.Identity.Name;
            int userId = userDao.GetUser(username).UserId;
            bool updated = orderDAO.UpdateOrderStatus(id, updateStatus, userId);
            email.OrderStatus(id);

            if (updated)
            {
                return updated;
            }
            else
            {
                return BadRequest("failed to update order status");
            }
        }
    }
}
