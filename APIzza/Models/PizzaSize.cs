namespace APIzza.Models
{
    public class PizzaSize
    {
        public string Size { get; set; }

        public PizzaSize() { }
        public PizzaSize(string size)
        {
            Size = size;
        }
        public decimal Price
        {
            get
            {
                switch (GetSizeOption())
                {
                    case PizzaOptions.SizeOption.Small:
                        return 8.99M;
                    case PizzaOptions.SizeOption.Medium:
                        return 10.99M;
                    case PizzaOptions.SizeOption.Large:
                        return 12.99M;
                    case PizzaOptions.SizeOption.ExtraLarge:
                        return 15.99M;
                    default:
                        return 0M;
                }
            }
        }

        public PizzaOptions.SizeOption GetSizeOption()
        {
            switch (Size.ToLower())
            {
                case "small":
                    return PizzaOptions.SizeOption.Small;
                case "medium":
                    return PizzaOptions.SizeOption.Medium;
                case "large":
                    return PizzaOptions.SizeOption.Large;
                case "xlarge":
                    return PizzaOptions.SizeOption.ExtraLarge;
                default:
                    throw new ArgumentException("Invalid pizza size");
            }
        }
    }
}
