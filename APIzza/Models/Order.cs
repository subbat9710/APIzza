namespace APIzza.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public decimal OrderCost { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderTime { get; set; }
        public string OrderType { get; set; }
        public int AddressId { get; set; }
        public int EmployeeId { get; set; }
        public string Username { get; set; }
    }
}
