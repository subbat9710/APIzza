namespace APIzza.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string AnonymousId { get; set; }
        public List<CartItems> Items { get; set; }
    }
}
