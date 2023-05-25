using APIzza.Models;
using Microsoft.Data.SqlClient;

namespace APIzza.DAO
{
    public class AddToCartSqlDao : IAddToCart
    {
        private readonly string connectionString;

        public AddToCartSqlDao(string dbconnectionString)
        {
            this.connectionString = dbconnectionString;
        }

        public CartItems AddItemToCart(string anonymousUserId, CartItems item)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Insert cartItem
                    string reviewSql = "INSERT INTO [CartItems] (ItemName, ItemPrice, ItemType, Quantity) " +
                        "VALUES(@ItemName, @ItemPrice, @ItemType, Quantity); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                    SqlCommand reviewCmd = new SqlCommand();
                    reviewCmd.CommandText = reviewSql;
                    reviewCmd.Connection = connection;

                    reviewCmd.Parameters.AddWithValue("@ItemName", item.Name);
                    reviewCmd.Parameters.AddWithValue("@ItemPrice", item.Price);
                    reviewCmd.Parameters.AddWithValue("@ItemType", item.Type);
                    reviewCmd.Parameters.AddWithValue("@Quantity", item.Quantity);

                    int review_id = (int)reviewCmd.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Did not enter the data correctly. {e.Message}");
            }
            return null;
        }

        public List<CartItems> CartItemList(string anonymousId)
        {
            List<CartItems> cartList = new List<CartItems>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("", conn);

                    string sql = "SELECT CartData FROM CartItems WHERE AnonymousUserId = @AnonymousUserId";

                    cmd.CommandText = sql;

                    cmd.Parameters.AddWithValue("@AnonymousUserId", anonymousId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CartItems newItem = CreateCartItemFromReader(reader);
                        cartList.Add(newItem);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return cartList;
        }

        public async Task<Cart> GetCartByAnonymousIdAsync(string anonymousId)
        {
            Cart cart = null;
            List<CartItems> cartItems = new List<CartItems>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("", conn);

                    string query = "SELECT c.Id AS CartId, " +
                                   "ci.Id AS ItemId, ci.ItemType, ci.ItemName, ci.Price, ci.Quantity " +
                                   "FROM CartItems ci " +
                                   "JOIN Cart c ON ci.CartId = c.Id " +
                                   "WHERE c.AnonymousUserId = @AnonymousUserId";

                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@AnonymousUserId", anonymousId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int cartId = (int)reader["CartId"];
                        int itemId = (int)reader["ItemId"];
                        string itemType = reader["ItemType"].ToString();
                        string itemName = reader["ItemName"].ToString();
                        decimal price = (decimal)reader["Price"];
                        int quantity = (int)reader["Quantity"];

                        if (cart == null)
                        {
                            cart = new Cart
                            {
                                Id = cartId,
                                AnonymousId = anonymousId,
                                Items = cartItems
                            };
                        }

                        cartItems.Add(new CartItems
                        {
                            Id = itemId,
                            Type = itemType,
                            Name = itemName,
                            Price = price,
                            Quantity = quantity
                        });
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return cart;
        }

        public async Task<int> CreateCartAsync(Cart cart)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;

                            // Insert the new cart into the database
                            command.CommandText = "INSERT INTO Cart (UserId, AnonymousUserId) VALUES (@UserId, @AnonymousUserId); SELECT CAST(scope_identity() AS int)";
                            command.Parameters.AddWithValue("@UserId", cart.UserId ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@AnonymousUserId", cart.AnonymousId ?? (object)DBNull.Value);
                            int cartId = (int)await command.ExecuteScalarAsync();

                            // Insert the cart items into the database
                            foreach (CartItems item in cart.Items)
                            {
                                command.CommandText = "INSERT INTO CartItems (CartId, ItemType, ItemName, Price, Quantity) VALUES (@CartId, @ItemType, @ItemName, @Price, @Quantity); SELECT CAST(scope_identity() AS int)";
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@CartId", cartId);
                                command.Parameters.AddWithValue("@ItemType", item.Type);
                                command.Parameters.AddWithValue("@ItemName", item.Name);
                                command.Parameters.AddWithValue("@Price", item.Price);
                                command.Parameters.AddWithValue("@Quantity", item.Quantity);
                                int cartItemId = (int)await command.ExecuteScalarAsync();

                                // Set the CartItem ID property
                                item.Id = cartItemId;
                            }
                        }

                        transaction.Commit();
                        // return the id of the newly created cart item
                        return cart.Items.FirstOrDefault()?.Id ?? 0;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Insert Cart if it doesn't exist
                    if (cart.Id == 0)
                    {
                        string insertCartSql = "INSERT INTO Cart (UserId, AnonymousUserId) VALUES (@UserId, @AnonymousUserId); SELECT CAST(SCOPE_IDENTITY() AS INT);";
                        SqlCommand insertCartCmd = new SqlCommand(insertCartSql, connection, transaction);
                        insertCartCmd.Parameters.AddWithValue("@UserId", cart.UserId ?? (object)DBNull.Value);
                        insertCartCmd.Parameters.AddWithValue("@AnonymousUserId", cart.AnonymousId);
                        cart.Id = (int)await insertCartCmd.ExecuteScalarAsync();
                    }
                    else
                    {
                        // Update Cart
                        string updateCartSql = "UPDATE Cart SET UserId = @UserId, AnonymousUserId = @AnonymousUserId WHERE Id = @Id";
                        SqlCommand updateCartCmd = new SqlCommand(updateCartSql, connection, transaction);
                        updateCartCmd.Parameters.AddWithValue("@Id", cart.Id);
                        updateCartCmd.Parameters.AddWithValue("@UserId", cart.UserId ?? (object)DBNull.Value);
                        updateCartCmd.Parameters.AddWithValue("@AnonymousUserId", cart.AnonymousId);
                        await updateCartCmd.ExecuteNonQueryAsync();
                    }

                    // Update CartItems for the cart
                    foreach (CartItems item in cart.Items)
                    {
                        string updateCartItemSql = "UPDATE CartItems SET Quantity = @Quantity WHERE CartId = @CartId AND ItemName = @ItemName";
                        SqlCommand updateCartItemCmd = new SqlCommand(updateCartItemSql, connection, transaction);
                        updateCartItemCmd.Parameters.AddWithValue("@CartId", cart.Id);
                        updateCartItemCmd.Parameters.AddWithValue("@ItemName", item.Name);
                        updateCartItemCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                        int rowsAffected = await updateCartItemCmd.ExecuteNonQueryAsync();

                        if (rowsAffected == 0)
                        {
                            string insertCartItemSql = "INSERT INTO CartItems (CartId, ItemType, ItemName, Price, Quantity) VALUES (@CartId, @ItemType, @ItemName, @Price, @Quantity); SELECT CAST(SCOPE_IDENTITY() AS INT);";
                            SqlCommand insertCartItemCmd = new SqlCommand(insertCartItemSql, connection, transaction);
                            insertCartItemCmd.Parameters.AddWithValue("@CartId", cart.Id);
                            insertCartItemCmd.Parameters.AddWithValue("@ItemType", item.Type);
                            insertCartItemCmd.Parameters.AddWithValue("@ItemName", item.Name);
                            insertCartItemCmd.Parameters.AddWithValue("@Price", item.Price);
                            insertCartItemCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                            item.Id = (int)await insertCartItemCmd.ExecuteScalarAsync();
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }


        public async Task<Cart> GetCartByUserIdAsync(string userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();

                    string sql = "SELECT Id, UserId, AnonymousUserId FROM Cart WHERE UserId = @UserId";

                    cmd.CommandText = sql;
                    cmd.Connection = connection;

                    cmd.Parameters.AddWithValue("@UserId", userId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Cart cart = new Cart()
                        {
                            Id = reader.GetInt32(0),
                            UserId = reader.GetString(1),
                            AnonymousId = reader.GetString(2)
                        };

                        return cart;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return null;
        }

        public List<CartItems> GetCartByUserAnonymousId(string anonymousUserId)
        {
            List<CartItems> cartList = new List<CartItems>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();

                    string sql = "SELECT c.Id AS CartId, " +
                                   "ci.Id AS ItemId, ci.ItemType, ci.ItemName, ci.Price, ci.Quantity " +
                                   "FROM CartItems ci " +
                                   "JOIN Cart c ON ci.CartId = c.Id " +
                                   "WHERE c.AnonymousUserId = @AnonymousUserId";

                    cmd.CommandText = sql;
                    cmd.Connection = connection;

                    cmd.Parameters.AddWithValue("@AnonymousUserId", anonymousUserId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        CartItems cartItems = CreateCartItemFromReader(reader);
                        cartList.Add(cartItems);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return cartList;
        }

        public bool RemoveItemFromCart(int itemId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    string sql = "DELETE FROM CartItems WHERE Id = @ItemId";
                    //  "AND CartId IN (SELECT Id FROM Cart WHERE AnonymousUserId = @AnonymousUserId);";
                    cmd.CommandText = sql;
                    cmd.Connection = connection;

                    cmd.Parameters.AddWithValue("@ItemId", itemId);
                    //  cmd.Parameters.AddWithValue("@AnonymousUserId", anonymousId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected == 1;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool ClearItemWhenCheckOut(string anonymousUserId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    string sql = "DELETE FROM CartItems " +
                        "WHERE CartId IN (SELECT Id FROM Cart WHERE AnonymousUserId = @AnonymousUserId)";

                    cmd.CommandText = sql;
                    cmd.Connection = connection;

                    cmd.Parameters.AddWithValue("@AnonymousUserId", anonymousUserId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected == 1;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public CartItems CreateCartItemFromReader(SqlDataReader reader)
        {
            CartItems cartItem = new CartItems()
            {
                Id = Convert.ToInt32(reader["ItemId"]),
                CartId = Convert.ToInt32(reader["CartId"]),
                Name = Convert.ToString(reader["ItemName"]),
                Price = Convert.ToDecimal(reader["Price"]),
                Quantity = Convert.ToInt32(reader["Quantity"]),
                Type = Convert.ToString(reader["ItemType"])
            };
            return cartItem;
        }

        public Cart CreateCartFromReader(SqlDataReader reader)
        {
            Cart cart = new Cart()
            {
                Id = Convert.ToInt32(reader["Id"]),
                UserId = Convert.ToString(reader["UserId"]),
                AnonymousId = Convert.ToString(reader["AnonymousUserId"]),
            };
            return cart;
        }
    }
}
