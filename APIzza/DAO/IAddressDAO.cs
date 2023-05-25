using APIzza.Models;

namespace APIzza.DAO
{
    public interface IAddressDAO
    {
        Address GetAddress(string addressID);
        Address AddAddress(Address addMe);
        Address EditAddress(Address update);
    }
}
