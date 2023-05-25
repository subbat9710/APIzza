namespace APIzza.Models
{
    public class PendingOrders : Order
    {
        public string Toppings { get; set; }
        public string Size { get; set; }
        public string CrustName { get; set; }
        public string SauceName { get; set; }
        public string SideName { get; set; }
        public string BeverageName { get; set; }
        public string SpecialtyName { get; set; }
        public int sideQuantity { get; set; }
        public int beverageQuantity { get; set; }
        public int specialtyQuantity { get; set; }
    }
}
