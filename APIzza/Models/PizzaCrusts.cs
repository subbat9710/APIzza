namespace APIzza.Models
{
    public class PizzaCrusts
    {
        public string Crusts { get; set; }

        public PizzaCrusts() { }
        public PizzaCrusts(string crusts)
        {
            Crusts = crusts;
        }
        public PizzaOptions.CrustsOptions GetCrustsOptions(string crusts)
        {
            switch (crusts.ToLower())
            {
                case "Hand Crust":
                    return PizzaOptions.CrustsOptions.HandCrust;
                case "Thin Crust":
                    return PizzaOptions.CrustsOptions.ThinCrust;
                default:
                    throw new ArgumentException("Invalid crusts options");
            }
        }
    }
}
