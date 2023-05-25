using APIzza.DAO;
using APIzza.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIzza.Controllers
{
    [Route("api/beverage")]
    [ApiController]
    public class BeverageController : Controller
    {
        private IBeverageDao beverageDao;
        private IUserDao userDao;

        public BeverageController(IBeverageDao _beverageDao, IUserDao _userDao)
        {
            this.beverageDao = _beverageDao;
            this.userDao = _userDao;
        }

        [HttpGet]
        public ActionResult<List<BeverageItem>> GetBeverage()
        {
            return Ok(beverageDao.GetBeverageItems());
        }
        [HttpPost]
        public ActionResult<BeverageItem> AddBeverage(BeverageItem beverageItem)
        {
            string username = User.Identity.Name;
            int userId = userDao.GetUser(username).UserId;
            beverageItem = beverageDao.AddBeverageItem(beverageItem, userId);

            return Created("/api/beverage/" + beverageItem.EmployeeId, beverageItem);
        }

        [HttpPut("{id}")]
        public ActionResult<bool> UpdatBeverage(int id, BeverageItem beverageItem)
        {
            if (id != beverageItem.ItemId)
            {
                return NotFound();
            }
            BeverageItem newSide = beverageDao.GetBeverageItemById(id);
            if (newSide == null)
            {
                return NotFound();
            }
            newSide.ItemName = beverageItem.ItemName;
            newSide.Description = beverageItem.Description;
            newSide.ImageUrl = beverageItem.ImageUrl;
            newSide.Price = beverageItem.Price;
            newSide.Available = beverageItem.Available;
            bool result = beverageDao.UpdateBeverage(id, beverageItem);

            return result;
        }

        [HttpDelete("{id}")]
        public ActionResult RemoveBeverage(int id)
        {
            bool result = beverageDao.RemoveBeverage(id);
            if (result)
            {
                return NoContent();
            }
            return StatusCode(500);
        }
    }
}
