using APIzza.Models;

namespace APIzza.DTO
{
    public class CartDto
    {
        public string AnonymousUserId { get; set; }
        public List<Pizza> Pizzas { get; set; } = new List<Pizza>();
        public List<Sides> Sides { get; set; } = new List<Sides>();
        public List<Beverages> Beverages { get; set; } = new List<Beverages>();
        public List<SpecialtyPizzaCart> SpecialtyPizzas { get; set; } = new List<SpecialtyPizzaCart>();
        public Address Address { get; set; }
        public Order CustomerOrder { get; set; }
        public decimal OrderCost
        {
            get
            {
                return Pizzas.Sum(pizza => pizza.Price * pizza.PizzaQuantity);
            }
        }
    }

    public class SpecialtyPizzaCart
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

    public class Sides
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
    public class Beverages
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

    public class ItemNames
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
