using APIzza.DTO;

namespace APIzza.BusinessLogic
{
    public interface ICartService
    {
        CartDto ProcessCheckout(CartDto cart);
    }
}
