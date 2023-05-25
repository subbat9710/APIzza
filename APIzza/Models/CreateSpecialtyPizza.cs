namespace APIzza.Models
{
    public class CreateSpecialtyPizza : Pizza
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public int EmployeeId { get; set; }
        public string ImageUrl { get; set; }
    }
}
