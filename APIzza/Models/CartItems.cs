namespace APIzza.Models
{
    public class CartItems
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
