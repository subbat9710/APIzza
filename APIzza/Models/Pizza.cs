namespace APIzza.Models
{
    public class Pizza
    {
        public int PizzaId { get; set; }
        public PizzaSize Size { get; set; }
        public List<PizzaTopping> Toppings { get; set; }
        public PizzaCrusts Crusts { get; set; }
        public PizzaSauces Sauces { get; set; }
        public int PizzaQuantity { get; set; } = 1;
        public decimal _Price { get; set; }
        public decimal Price
        {
            get
            {
                if (_Price == 0)
                {
                    decimal totalPizzaPrice = (Size.Price + Toppings.Count * 1.5M) * PizzaQuantity;
                    return totalPizzaPrice;

                }
                else
                {
                    return _Price;
                }
            }
        }
    }
}
