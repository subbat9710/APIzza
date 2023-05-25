using APIzza.Models;

namespace APIzza.DAO
{
    public interface ISideDao
    {
        SideItem AddSideItem(SideItem sideItem, int userId);
        List<SideItem> GetSideItems();
        bool RemoveSide(int sideId);
        bool UpdateSide(int sideId, SideItem sideItem);
        SideItem GetSideItemById(int sideId);
    }
}
