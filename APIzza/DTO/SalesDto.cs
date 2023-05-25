namespace APIzza.DTO
{
    public class SalesDto
    {
        private const string DateFormat = "MM/dd/yyyy";
        public string OrderDate { get; set; }
        public decimal SpecialtySale { get; set; }
        public decimal SidesSale { get; set; }
        public decimal BeverageSale { get; set; }
        public string OrderType { get; set; }

        public void SetOrderDate(DateTime orderDate)
        {
            this.OrderDate = orderDate.ToString(DateFormat);
        }
    }
}
