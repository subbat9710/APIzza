namespace APIzza.Models
{
    public class Address
    {
        public string HomeType { get; set; }
        public string StreetAddress { get; set; }
        public string Apartment { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string Zip { get; set; }
        public int AddressID { get; set; }
        public string Instructions { get; set; }
    }
}
