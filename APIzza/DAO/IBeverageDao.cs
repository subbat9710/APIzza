using APIzza.Models;

namespace APIzza.DAO
{
    public interface IBeverageDao
    {
        BeverageItem AddBeverageItem(BeverageItem beverageItem, int userId);
        List<BeverageItem> GetBeverageItems();
        bool RemoveBeverage(int beverageId);
        BeverageItem GetBeverageItemById(int beverageId);
        bool UpdateBeverage(int sideId, BeverageItem beverageItem);
    }
}
