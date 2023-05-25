using APIzza.DAO;
using APIzza.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIzza.Controllers
{
    [Route("api/specialty")]
    [ApiController]
    public class SpecialtyController : Controller
    {
        public readonly ISpecialtyDAO specialtyDAO;
        public SpecialtyController(ISpecialtyDAO _specialtyDAO)
        {
            this.specialtyDAO = _specialtyDAO;
        }

        [HttpPut]
        public ActionResult<SpecialtyPizza> EditSpecialty(SpecialtyPizza updateSpecialty)
        {
            return EditSpecialty(updateSpecialty);
        }

        //Get the list of all the available Specialty Pizzas
        [HttpGet]
        public ActionResult<List<SpecialtyPizza>> GetAvailableSpecialtyPizza()
        {
            return Ok(specialtyDAO.GetAllAvailableSpecialtyPizza());
        }

    }
}
