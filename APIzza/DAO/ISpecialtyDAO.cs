using APIzza.Models;

namespace APIzza.DAO
{
    public interface ISpecialtyDAO
    {
        IList<SpecialtyPizza> GetAllAvailableSpecialtyPizza();
        List<SpecialtyPizza> GetSpecialtyPizzasByIds(List<int> ids);
    }
}
