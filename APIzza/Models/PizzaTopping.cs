namespace APIzza.Models
{
    public class PizzaTopping
    {
        public string Topping { get; set; }
        public decimal Price
        {
            get
            {
                return 1.50M;
            }
        }

        public PizzaTopping() { }

        public PizzaOptions.ToppingsOption ToToppingsOption()
        {
            Enum.TryParse(Topping, out PizzaOptions.ToppingsOption option);
            return option;
        }
    }
}
