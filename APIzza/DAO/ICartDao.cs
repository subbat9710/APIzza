using APIzza.DTO;
using APIzza.Models;

namespace APIzza.DAO
{
    public interface ICartDao
    {
        CartDto GetCustomPizza(CartDto cartDto, int orderId);
        CartDto GetOrders(CartDto cartDto, decimal orderCost);
        CartDto GetAddress(CartDto cartDto);
        List<SpecialtyPizza> GetSpecialtyPizzasByNames(List<string> names);
        List<ItemNames> GetSidesByNames(List<string> names);
        List<ItemNames> GetBeveragesByNames(List<string> names);
        CartDto GetSidesOrder(CartDto cartDto, int orderId, int sideQuantity);
        CartDto GetBeverageOrder(CartDto cartDto, int orderId, int beverageQuantity);
        CartDto GetSpecialtyOrder(CartDto cartDto, int orderId, int specialtyQuantity);
    }
}
