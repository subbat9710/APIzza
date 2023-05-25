using APIzza.Models;
using Microsoft.Data.SqlClient;

namespace APIzza.DAO
{
    public class BeverageSqlDao : IBeverageDao
    {
        private readonly string connectionString;
        public BeverageSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public BeverageItem AddBeverageItem(BeverageItem beverageItem, int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create Beverages List
                    string sql = "INSERT INTO CreateBeverage (beveragePrice, beverageName, isAvailable, employeeId, imageUrl, itemType, description) " +
                        "VALUES(@beveragePrice, @beverageName, @isAvailable, @employeeId, @imageUrl, @itemType, @description); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                    SqlCommand Cmd = new SqlCommand();
                    Cmd.CommandText = sql;
                    Cmd.Connection = connection;

                    Cmd.Parameters.AddWithValue("@beverageName", beverageItem.ItemName);
                    Cmd.Parameters.AddWithValue("@beveragePrice", beverageItem.Price);
                    Cmd.Parameters.AddWithValue("@isAvailable", beverageItem.Available);
                    Cmd.Parameters.AddWithValue("@employeeId", userId);
                    Cmd.Parameters.AddWithValue("@imageUrl", beverageItem.ImageUrl);
                    Cmd.Parameters.AddWithValue("@itemType", beverageItem.ItemType);
                    Cmd.Parameters.AddWithValue("@description", beverageItem.Description);
                    int beverageId = (int)Cmd.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Did not enter the data correctly. {e.Message}");
            }
            return beverageItem;
        }

        public List<BeverageItem> GetBeverageItems()
        {
            List<BeverageItem> result = new List<BeverageItem>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT beverageId, beverageName, beveragePrice, imageUrl, itemType, description " +
                        "FROM CreateBeverage;";
                    cmd.CommandText = sql;
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        BeverageItem item = new BeverageItem();
                        item = CreateBeveratesFromReader(reader);
                        result.Add(item);
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to view the pendingOrders {e.Message}");
            }
            return result;
        }

        public BeverageItem GetBeverageItemById(int beverageId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT beverageId, beverageName, beveragePrice, imageUrl, itemType, description, isAvailable " +
                        "FROM CreateBeverage WHERE beverageId = @beverageId;";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;

                    cmd.Parameters.AddWithValue("@beverageId", beverageId);

                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        BeverageItem result = CreateBeveratesFromReader(reader);
                        return result;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return null;
        }

        public bool RemoveBeverage(int beverageId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    string sql = "DELETE FROM CreateBeverage WHERE beverageId = @beverageId;";
                    cmd.CommandText = sql;
                    cmd.Connection = connection;

                    cmd.Parameters.AddWithValue("@beverageId", beverageId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected == 1;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool UpdateBeverage(int sideId, BeverageItem beverageItem)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE CreateBeverage SET beverageName = @beverageName, beveragePrice = @beveragePrice, imageUrl = @imageUrl, description = @description, isAvailable = @isAvailable WHERE beverageId = @beverageId;";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;

                    cmd.Parameters.AddWithValue("@beverageId", sideId);
                    cmd.Parameters.AddWithValue("@beverageName", beverageItem.ItemName);
                    cmd.Parameters.AddWithValue("@beveragePrice", beverageItem.Price);
                    cmd.Parameters.AddWithValue("@imageUrl", beverageItem.ImageUrl);
                    cmd.Parameters.AddWithValue("@description", beverageItem.Description);
                    cmd.Parameters.AddWithValue("@isAvailable", beverageItem.Available);

                    cmd.Connection = connection;

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return (rowsAffected > 0);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private BeverageItem CreateBeveratesFromReader(SqlDataReader reader)
        {
            BeverageItem items = new BeverageItem();
            items.ItemId = Convert.ToInt32(reader["beverageId"]);
            items.ItemName = Convert.ToString(reader["beverageName"]);
            items.Description = Convert.ToString(reader["description"]);
            items.ImageUrl = Convert.ToString(reader["imageUrl"]);
            items.ItemType = Convert.ToString(reader["itemType"]);
            items.Price = Convert.ToDecimal(reader["beveragePrice"]);

            return items;
        }
    }
}
