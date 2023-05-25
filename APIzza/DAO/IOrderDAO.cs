namespace APIzza.DAO
{
    public interface IOrderDAO
    {
        string GetEmail(int orderId);
        string GetOrderStatus(int orderId);
        //Create an Order object from the database
        bool UpdateOrderStatus(int orderId, string updatedStatus, int userId);
    }
}
