namespace APIzza.Models
{
    public class PizzaSauces
    {
        public string Sauces { get; set; }

        public PizzaSauces() { }

        public PizzaSauces(string sauces)
        {
            Sauces = sauces;
        }
        public PizzaOptions.SaucesOptions GetSaucesOptions(string sauces)
        {
            switch (sauces.ToLower())
            {
                case "Azure":
                    return PizzaOptions.SaucesOptions.Azure;
                case "Red":
                    return PizzaOptions.SaucesOptions.Red;
                case "White":
                    return PizzaOptions.SaucesOptions.White;
                default:
                    throw new ArgumentException("Invalid Sauces options");
            }
        }
    }
}
