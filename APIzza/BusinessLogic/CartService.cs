using APIzza.DAO;
using APIzza.DTO;
using APIzza.Models;

namespace APIzza.BusinessLogic
{
    public class CartService : ICartService
    {
        private ICartDao cartDao;

        public CartService(ICartDao _cartDao)
        {
            this.cartDao = _cartDao;
        }

        public CartDto ProcessCheckout(CartDto cart)
        {

            if (cart.SpecialtyPizzas.Count == 0 && cart.Sides.Count == 0 && cart.Pizzas.Count == 0 && cart.Beverages.Count == 0)
            {
                throw new Exception("You don't have any items in the cart");
            }

            int specialtyQuantity = 0;
            if (cart.SpecialtyPizzas.Count > 0)
            {
                List<string> specialtyPizzaNames = new List<string>();
                List<SpecialtyPizzaCart> expandedSpecialty = new List<SpecialtyPizzaCart>();

                foreach (SpecialtyPizzaCart items in cart.SpecialtyPizzas)
                {
                    if (items.Name != null)
                    {
                        specialtyPizzaNames.Add(items.Name);

                        for (int i = 0; i < items.Quantity; i++)
                        {
                            expandedSpecialty.Add(items);
                        }
                    }
                }
                specialtyQuantity = expandedSpecialty.Count;

                List<SpecialtyPizza> specialtyPizzas = cartDao.GetSpecialtyPizzasByNames(specialtyPizzaNames);

                foreach (SpecialtyPizza pizza in specialtyPizzas)
                {
                    if (cart.CustomerOrder != null)
                    {
                        decimal specialtyCost = expandedSpecialty.Count(s => s.Name == pizza.Name) * pizza.Price;
                        cart.CustomerOrder.OrderCost += specialtyCost;
                    }
                }
            }

            int sideQuantity = 0;
            if (cart.Sides.Count > 0)
            {
                List<string> itemNames = new List<string>();
                List<Sides> expandedSides = new List<Sides>();

                foreach (Sides items in cart.Sides)
                {
                    if (items.Name != null)
                    {
                        itemNames.Add(items.Name);

                        // adding multiple copies of the side based on its quantity
                        for (int i = 0; i < items.Quantity; i++)
                        {
                            expandedSides.Add(items);
                        }
                    }
                }
                sideQuantity = expandedSides.Count;

                List<ItemNames> sidesItems = cartDao.GetSidesByNames(itemNames);

                foreach (ItemNames items in sidesItems)
                {
                    if (cart.CustomerOrder != null)
                    {
                        // calculating the total cost of each item in the expandedSides list
                        decimal sideCost = expandedSides.Count(s => s.Name == items.Name) * items.Price;
                        cart.CustomerOrder.OrderCost += sideCost;
                    }
                }
            }

            int beverageQuantity = 0;
            if (cart.Beverages.Count > 0)
            {
                List<string> itemNames = new List<string>();
                List<Beverages> expandedBeverages = new List<Beverages>();

                foreach (Beverages items in cart.Beverages)
                {
                    if (items.Name != null)
                    {
                        itemNames.Add(items.Name);

                        for (int i = 0; i < items.Quantity; i++)
                        {
                            expandedBeverages.Add(items);
                        }
                    }
                }

                beverageQuantity = expandedBeverages.Count;

                List<ItemNames> beverageItems = cartDao.GetBeveragesByNames(itemNames);

                foreach (ItemNames items in beverageItems)
                {
                    if (cart.CustomerOrder != null)
                    {
                        decimal beverageCost = expandedBeverages.Count(b => b.Name == items.Name) * items.Price;
                        cart.CustomerOrder.OrderCost += beverageCost;
                    }
                }

            }

            if (cart.CustomerOrder.OrderType == "Delivery")
            {
                cartDao.GetAddress(cart);
            }

            cartDao.GetOrders(cart, cart.CustomerOrder.OrderCost + cart.OrderCost);

            EmailConfirmation.EmailNotifications(cart);

            int orderId = cart.CustomerOrder.OrderId;

            //inserting specialty orders
            if (cart.SpecialtyPizzas.Count > 0)
            {
                cartDao.GetSpecialtyOrder(cart, orderId, specialtyQuantity);
            }
            //inserting side orders
            if (cart.Sides.Count > 0)
            {
                cartDao.GetSidesOrder(cart, orderId, sideQuantity);
            }
            //inserting beverage orders
            if (cart.Beverages.Count > 0)
            {
                cartDao.GetBeverageOrder(cart, orderId, beverageQuantity);
            }
            //inserting custom pizza orders
            if (cart.Pizzas.Count > 0)
            {
                cartDao.GetCustomPizza(cart, orderId);
            }
            return cart;
        }
    }
}
