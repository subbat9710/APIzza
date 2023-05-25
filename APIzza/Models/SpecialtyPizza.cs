namespace APIzza.Models
{
    public class SpecialtyPizza : Pizza
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public int EmployeeId { get; set; }
        public string ImageUrl { get; set; }
        public string PizzaSize { get; set; }
        public string PizzaSauce { get; set; }
        public string PizzaCrust { get; set; }
        public string PizzaToppings { get; set; }
        public int CreatePizzaId { get; set; }
    }
}
