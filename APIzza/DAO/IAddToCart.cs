using APIzza.Models;

namespace APIzza.DAO
{
    public interface IAddToCart
    {
        CartItems AddItemToCart(string anonymousUserId, CartItems item);
        List<CartItems> CartItemList(string anonymousId);
        Task<Cart> GetCartByAnonymousIdAsync(string anonymousId);
        Task UpdateCartAsync(Cart cart);
        Task<int> CreateCartAsync(Cart cart);
        Task<Cart> GetCartByUserIdAsync(string userId);
        List<CartItems> GetCartByUserAnonymousId(string anonymousUserId);
        bool RemoveItemFromCart(int itemId);
        bool ClearItemWhenCheckOut(string anonymousUserId);
    }
}
