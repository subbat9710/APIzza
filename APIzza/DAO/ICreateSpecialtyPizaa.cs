using APIzza.Models;

namespace APIzza.DAO
{
    public interface ICreateSpecialtyPizaa
    {
        CreateSpecialtyPizza CreateSpecialtyPizza(CreateSpecialtyPizza createSpecialty, int userId);
        bool ChangeAvailability(int createId, bool isAvailable);
        bool EditSpecialtyPizza(int createId, CreateSpecialtyPizza specialtyPizza);
        bool RemoveSpecialtyPizza(int createId);
        SpecialtyPizza GetSpecialtyPizzaById(int createId);
    }
}
