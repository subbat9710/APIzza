using APIzza.DAO;
using APIzza.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIzza.Controllers
{
    [Route("api/createspecialty")]
    [ApiController]
    public class CreateSpecialtyController : ControllerBase
    {
        private ICreateSpecialtyPizaa createSpecialtyDao;
        private IUserDao userDao;

        public CreateSpecialtyController(ICreateSpecialtyPizaa _createSpecialtyDao, IUserDao _userDao)
        {
            this.createSpecialtyDao = _createSpecialtyDao;
            this.userDao = _userDao;
        }

        // Employee can able to create the specialty Pizza with size options, crust options, sauce options and toppings options
        [HttpPost]
        public ActionResult<CreateSpecialtyPizza> CreateSpecialtyPizza(CreateSpecialtyPizza createSpecialtyPizza)
        {
            string username = User.Identity.Name;
            int userId = userDao.GetUser(username).UserId;

            //  createSpecialtyPizza.Size = new PizzaSize(createSpecialtyPizza.Size.Size);

            createSpecialtyPizza = createSpecialtyDao.CreateSpecialtyPizza(createSpecialtyPizza, userId);

            return Created("api/createspecialty/" + createSpecialtyPizza.PizzaId, createSpecialtyPizza);
        }

        //Employee can able to change the availability of the specialty pizza
        [HttpPut("{id}/availability")]
        public ActionResult<CreateSpecialtyPizza> UpdateAvailability(int id, bool isAvailability)
        {
            bool changedAvailability = createSpecialtyDao.ChangeAvailability(id, isAvailability);

            if (changedAvailability)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to change the availability");
            }
        }

        //Employee Can able to update the price, size, crusts,sauces etc
        [HttpPut("{id}")]
        public ActionResult<CreateSpecialtyPizza> EditSpecialtyPizza(int id, CreateSpecialtyPizza createSpecialtyPizza)
        {
            bool editSpecialtyPizza = createSpecialtyDao.EditSpecialtyPizza(id, createSpecialtyPizza);

            if (editSpecialtyPizza)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to edit the availability");
            }
        }

        //Remove SpecialtyPizza from the List
        [HttpDelete("{id}")]
        public ActionResult<CreateSpecialtyPizza> RemoveSpecialtyPizza(int id)
        {
            bool result = createSpecialtyDao.RemoveSpecialtyPizza(id);
            if (result)
            {
                return NoContent();
            }
            return StatusCode(500);
        }

        [HttpGet("{id}")]
        public ActionResult<SpecialtyPizza> GetSpecialtyById(int id)
        {
            return Ok(createSpecialtyDao.GetSpecialtyPizzaById(id));
        }
    }
}
