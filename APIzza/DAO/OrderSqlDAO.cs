using Microsoft.Data.SqlClient;

namespace APIzza.DAO
{
    public class OrderSqlDAO : IOrderDAO
    {
        private readonly string connectionString;

        public OrderSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public string GetEmail(int orderId)
        {
            string email = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT email from OrderPizza where orderId = @orderId;", conn);
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        email = reader.GetString(0);
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }

            return email;
        }

        public string GetOrderStatus(int orderId)
        {
            string orderStatus = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT orderStatus from OrderPizza where orderId = @orderId;", conn);
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        orderStatus = reader.GetString(0);
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }

            return orderStatus;
        }

        public bool UpdateOrderStatus(int orderId, string updatedStatus, int userId)
        {
            try
            {
                int rowsAffected;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE OrderPizza SET orderStatus = @updatedStatus, employeeId = @employeeId WHERE orderId = @orderId;");
                    cmd.Parameters.AddWithValue("@updatedStatus", updatedStatus);
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    cmd.Parameters.AddWithValue("@employeeId", userId);
                    cmd.Connection = conn;
                    rowsAffected = cmd.ExecuteNonQuery();

                }
                return (rowsAffected == 1);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
