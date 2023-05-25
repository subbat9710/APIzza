using APIzza.BusinessLogic;
using APIzza.DAO;
using APIzza.DTO;
using APIzza.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIzza.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        private readonly IAddToCart addToCart;

        public CartController(ICartService _cartService, IAddToCart _addToCart)
        {
            this.cartService = _cartService;
            this.addToCart = _addToCart;
        }

        [HttpPost("checkout")]
        public ActionResult<CartDto> Checkout(CartDto cart)
        {
            try
            {
                cartService.ProcessCheckout(cart);
                return Created("/api/cart/" + cart.CustomerOrder.OrderId, cart);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("add-to-cart")]
        public async Task<ActionResult> AddToCart(Cart cart)
        {
            try
            {
                string userId = null;
                string anonymousId = cart.AnonymousId;
                ClaimsPrincipal currentUser = this.User;
                if (currentUser.Identity.IsAuthenticated)
                {
                    userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                }
                if (anonymousId == null)
                {
                    anonymousId = GetAnonymousId();
                }
                else
                {
                    anonymousId = cart.AnonymousId;
                }

                Cart existingCart = null;
                if (userId != null)
                {
                    existingCart = await addToCart.GetCartByUserIdAsync(userId);
                }
                else if (anonymousId != null)
                {
                    existingCart = await addToCart.GetCartByAnonymousIdAsync(anonymousId);
                }

                if (existingCart == null)
                {
                    if (userId != null)
                    {
                        cart.UserId = userId;
                    }
                    else if (anonymousId != null)
                    {
                        cart.AnonymousId = anonymousId;
                    }

                    await addToCart.CreateCartAsync(cart);
                    var newItem = cart.Items[0];

                    return Created("/api/add-to-cart" + cart.Id, new { CartId = cart.Id, CartItemId = newItem.Id });
                }
                else
                {
                    var existingItem = existingCart.Items.FirstOrDefault(item => item.Type == cart.Items[0].Type && item.Name == cart.Items[0].Name && item.Price == cart.Items[0].Price);
                    if (existingItem != null)
                    {
                        existingItem.Quantity += cart.Items[0].Quantity;
                        await addToCart.UpdateCartAsync(existingCart);
                        var updatedItem = existingCart.Items.FirstOrDefault(item => item.Type == cart.Items[0].Type && item.Name == cart.Items[0].Name && item.Price == cart.Items[0].Price);

                        return Created("/api/add-to-cart" + existingCart.Id, new { CartId = existingCart.Id, CartItemId = updatedItem.Id });
                    }
                    else
                    {
                        existingCart.Items.AddRange(cart.Items);
                        await addToCart.UpdateCartAsync(existingCart);
                        var newItem = existingCart.Items.Last();

                        return Created("/api/add-to-cart" + existingCart.Id, new { CartId = existingCart.Id, CartItemId = newItem.Id });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("get-cart-by-anonymous-id/{anonymousId}")]
        public ActionResult<List<CartItems>> GetCartByAnonymousIdAsync(string anonymousId)
        {
            try
            {
                Cart cart = new Cart();
                if (cart == null)
                {
                    return NotFound();
                }

                return addToCart.GetCartByUserAnonymousId(anonymousId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveItemFromCart(int id)
        {
            bool result = addToCart.RemoveItemFromCart(id);
            if (result)
            {
                return NoContent();
            }
            return StatusCode(500);
        }

        [HttpDelete("clear-cart/{anonymousUserId}")]
        public IActionResult ClearCartWhenCheckOut(string anonymousUserId)
        {
            bool result = addToCart.ClearItemWhenCheckOut(anonymousUserId);
            if (result)
            {
                return NoContent();
            }
            return StatusCode(500);
        }

        private string GetAnonymousId()
        {
            string anonymousId = HttpContext.Session.GetString("AnonymousId");

            if (string.IsNullOrEmpty(anonymousId))
            {
                anonymousId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("AnonymousId", anonymousId);
            }
            return anonymousId;
        }
    }
}
