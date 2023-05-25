using APIzza.DAO;
using APIzza.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIzza.Controllers
{
    [Route("api/sides")]
    [ApiController]
    public class SideController : ControllerBase
    {
        private ISideDao sideDao;
        private IUserDao userDao;

        public SideController(ISideDao _sideDao, IUserDao _userDao)
        {
            this.sideDao = _sideDao;
            this.userDao = _userDao;
        }

        [HttpGet]
        public ActionResult<List<SideItem>> GetSide()
        {
            return Ok(sideDao.GetSideItems());
        }
        [HttpPost]
        public ActionResult<SideItem> AddSide(SideItem sideItem)
        {
            string username = User.Identity.Name;
            int userId = userDao.GetUser(username).UserId;
            sideItem = sideDao.AddSideItem(sideItem, userId);

            return Created("/api/side/" + sideItem.EmployeeId, sideItem);
        }

        [HttpPut("{id}")]
        public ActionResult<bool> UpdateSide(int id, SideItem sideItem)
        {
            if (id != sideItem.ItemId)
            {
                return NotFound();
            }
            SideItem newSide = sideDao.GetSideItemById(id);
            if (newSide == null)
            {
                return NotFound();
            }
            newSide.ItemName = sideItem.ItemName;
            newSide.Description = sideItem.Description;
            newSide.ImageUrl = sideItem.ImageUrl;
            newSide.Price = sideItem.Price;
            newSide.Available = sideItem.Available;
            bool result = sideDao.UpdateSide(id, sideItem);

            return result;
        }

        [HttpDelete("{id}")]
        public ActionResult RemoveSide(int id)
        {
            bool result = sideDao.RemoveSide(id);
            if (result)
            {
                return NoContent();
            }
            return StatusCode(500);
        }
    }
}
