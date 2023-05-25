using APIzza.Models;
using Microsoft.Data.SqlClient;

namespace APIzza.DAO
{
    public class SideSqlDao : ISideDao
    {
        private readonly string connectionString;

        public SideSqlDao(string dbconnectionString)
        {
            this.connectionString = dbconnectionString;
        }


        public SideItem AddSideItem(SideItem sideItem, int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create Side List
                    string sql = "INSERT INTO CreateSides (sideName, sidePrice, isAvailable, employeeId, imageUrl, itemType, description) " +
                        "VALUES(@sideName, @sidePrice, @isAvailable, @employeeId, @imageUrl, @itemType, @description); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                    SqlCommand Cmd = new SqlCommand();
                    Cmd.CommandText = sql;
                    Cmd.Connection = connection;

                    Cmd.Parameters.AddWithValue("@sideName", sideItem.ItemName);
                    Cmd.Parameters.AddWithValue("@sidePrice", sideItem.Price);
                    Cmd.Parameters.AddWithValue("@isAvailable", sideItem.Available);
                    Cmd.Parameters.AddWithValue("@employeeId", userId);
                    Cmd.Parameters.AddWithValue("@imageUrl", sideItem.ImageUrl);
                    Cmd.Parameters.AddWithValue("@itemType", sideItem.ItemType);
                    Cmd.Parameters.AddWithValue("@description", sideItem.Description);
                    int sideId = (int)Cmd.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Did not enter the data correctly. {e.Message}");
            }
            return sideItem;
        }

        public List<SideItem> GetSideItems()
        {
            List<SideItem> result = new List<SideItem>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT sideId, sideName, sidePrice, imageUrl, itemType, description, isAvailable " +
                        "FROM CreateSides;";
                    cmd.CommandText = sql;
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        SideItem item = new SideItem();
                        item = CreateSideFromReader(reader);
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

        public SideItem GetSideItemById(int sideId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT sideId, sideName, sidePrice, imageUrl, itemType, description, isAvailable " +
                        "FROM CreateSides WHERE sideId = @sideId;";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;

                    cmd.Parameters.AddWithValue("@sideId", sideId);

                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        SideItem result = CreateSideFromReader(reader);
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

        public bool RemoveSide(int sideId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    string sql = "DELETE FROM CreateSides WHERE sideId = @sideId;";
                    cmd.CommandText = sql;
                    cmd.Connection = connection;

                    cmd.Parameters.AddWithValue("@sideId", sideId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected == 1;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool UpdateSide(int sideId, SideItem sideItem)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE CreateSides SET sideName = @sideName, sidePrice = @sidePrice, imageUrl = @imageUrl, description = @description, isAvailable = @isAvailable WHERE sideId = @sideId;";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;

                    cmd.Parameters.AddWithValue("@sideId", sideId);
                    cmd.Parameters.AddWithValue("@sideName", sideItem.ItemName);
                    cmd.Parameters.AddWithValue("@sidePrice", sideItem.Price);
                    cmd.Parameters.AddWithValue("@imageUrl", sideItem.ImageUrl);
                    cmd.Parameters.AddWithValue("@description", sideItem.Description);
                    cmd.Parameters.AddWithValue("@isAvailable", sideItem.Available);

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
        private SideItem CreateSideFromReader(SqlDataReader reader)
        {
            SideItem items = new SideItem();
            items.ItemId = Convert.ToInt32(reader["sideId"]);
            items.ItemName = Convert.ToString(reader["sideName"]);
            items.Description = Convert.ToString(reader["description"]);
            items.ImageUrl = Convert.ToString(reader["imageUrl"]);
            items.ItemType = Convert.ToString(reader["itemType"]);
            items.Price = Convert.ToDecimal(reader["sidePrice"]);
            items.Available = Convert.ToBoolean(reader["isAvailable"]);

            return items;
        }
    }
}
