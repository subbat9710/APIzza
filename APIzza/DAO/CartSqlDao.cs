using APIzza.DTO;
using APIzza.Models;
using Microsoft.Data.SqlClient;

namespace APIzza.DAO
{
    public class CartSqlDao : ICartDao
    {
        private readonly string connectionString;

        public CartSqlDao(string dbconnectionString)
        {
            this.connectionString = dbconnectionString;
        }
        public CartDto GetAddress(CartDto cartDto)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Insert Address
                    string deliberySql = "INSERT INTO [Address] (homeType, streetAddress, state, zipCode, apartmentNumber, instructions, city) " +
                        "VALUES(@homeType, @streetAddress, @state, @zipCode, @apartmentNumber, @instructions, @city); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                    SqlCommand deliberyCmd = new SqlCommand();
                    deliberyCmd.CommandText = deliberySql;
                    deliberyCmd.Connection = connection;

                    deliberyCmd.Parameters.AddWithValue("@homeType", cartDto.Address.HomeType);
                    deliberyCmd.Parameters.AddWithValue("@streetAddress", cartDto.Address.StreetAddress);
                    deliberyCmd.Parameters.AddWithValue("@state", cartDto.Address.StateAbbreviation);
                    deliberyCmd.Parameters.AddWithValue("@zipCode", cartDto.Address.Zip);
                    deliberyCmd.Parameters.AddWithValue("@apartmentNumber", cartDto.Address.Apartment);
                    deliberyCmd.Parameters.AddWithValue("@instructions", cartDto.Address.Instructions);
                    deliberyCmd.Parameters.AddWithValue("@city", cartDto.Address.City);
                    int addressId = (int)deliberyCmd.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Did not enter the data correctly. {e.Message}");
            }
            return cartDto;
        }
        public CartDto GetOrders(CartDto cartDto, decimal orderCost)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Insert Order
                    string orderSql = "INSERT INTO [OrderPizza] (orderCost, email, phoneNumber, orderStatus, orderDate, employeeId, addressId, ordertype) " +
                        "VALUES(@orderCost, @email, @phoneNumber, @orderStatus, @orderDate, @employeeId, @addressId, @ordertype); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                    SqlCommand orderCmd = new SqlCommand();
                    orderCmd.CommandText = orderSql;
                    orderCmd.Connection = connection;

                    orderCmd.Parameters.AddWithValue("@orderCost", orderCost);
                    orderCmd.Parameters.AddWithValue("@email", cartDto.CustomerOrder.Email);
                    orderCmd.Parameters.AddWithValue("@phoneNumber", cartDto.CustomerOrder.PhoneNumber);
                    orderCmd.Parameters.AddWithValue("@orderStatus", "Pending");
                    orderCmd.Parameters.AddWithValue("@orderDate", DateTime.Now);
                    orderCmd.Parameters.AddWithValue("@employeeId", 1);
                    orderCmd.Parameters.AddWithValue("@addressId", 1);
                    orderCmd.Parameters.AddWithValue("@ordertype", cartDto.CustomerOrder.OrderType);
                    int orderId = (int)orderCmd.ExecuteScalar();
                    cartDto.CustomerOrder.OrderId = orderId;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Did not enter the data correctly. {e.Message}");
            }
            return cartDto;
        }
        public CartDto GetCustomPizza(CartDto cartDto, int orderId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (Pizza items in cartDto.Pizzas)
                    {
                        // Insert size
                        string sizeSql = "INSERT INTO Size (size, sizePrice) VALUES (@size, @sizePrice); " +
                                         "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                        SqlCommand sizeCmd = new SqlCommand(sizeSql, connection);
                        sizeCmd.Parameters.AddWithValue("@size", items.Size.Size.ToString());
                        sizeCmd.Parameters.AddWithValue("@sizePrice", items.Size.Price);
                        int sizeId = (int)sizeCmd.ExecuteScalar();

                        // Insert Crusts
                        string crustsSql = "INSERT INTO Crust (crustName) VALUES (@crustName); " +
                                           "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                        SqlCommand crustsCmd = new SqlCommand(crustsSql, connection);
                        crustsCmd.Parameters.AddWithValue("@crustName", items.Crusts.Crusts.ToString());
                        int crustsId = (int)crustsCmd.ExecuteScalar();

                        //Insert Sauces
                        string saucesSql = "INSERT INTO Sauce (sauceName) VALUES (@sauceName); " +
                            "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                        SqlCommand saucesCmd = new SqlCommand(saucesSql, connection);
                        saucesCmd.Parameters.AddWithValue("@sauceName", items.Sauces.Sauces.ToString());
                        int saucesId = (int)saucesCmd.ExecuteScalar();


                        // Insert pizza
                        string pizzaSql = "INSERT INTO Pizza (sizeId, orderId, price, sauceId, crustId, pizzaQuantity) " +
                            "VALUES (@sizeId, @orderId, @price, @sauceId, @crustId, @pizzaQuantity);" +
                                           "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                        SqlCommand pizzaCmd = new SqlCommand(pizzaSql, connection);
                        pizzaCmd.Parameters.AddWithValue("@sizeId", sizeId);
                        pizzaCmd.Parameters.AddWithValue("@orderId", orderId);
                        pizzaCmd.Parameters.AddWithValue("@price", items.Price);
                        pizzaCmd.Parameters.AddWithValue("@sauceId", saucesId);
                        pizzaCmd.Parameters.AddWithValue("@crustId", crustsId);
                        pizzaCmd.Parameters.AddWithValue("@pizzaQuantity", items.PizzaQuantity);
                        int pizzaId = (int)pizzaCmd.ExecuteScalar();

                        items.PizzaId = pizzaId;

                        // Insert toppings
                        foreach (PizzaTopping topping in items.Toppings)
                        {
                            string toppingsSql = "INSERT INTO Topping (toppingName, toppingPrice) VALUES (@toppingName, @toppingPrice); " +
                                              "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                            SqlCommand toppingsCmd = new SqlCommand(toppingsSql, connection);
                            toppingsCmd.Parameters.AddWithValue("@toppingName", topping.Topping.ToString());
                            toppingsCmd.Parameters.AddWithValue("@toppingPrice", topping.Price);
                            int toppingId = (int)toppingsCmd.ExecuteScalar();

                            // Insert into PizzaToppings table
                            string pizzaToppingsSql = "INSERT INTO PizzaTopping (pizzaId, toppingId) VALUES (@pizzaId, @toppingId);";
                            SqlCommand pizzaToppingsCmd = new SqlCommand(pizzaToppingsSql, connection);
                            pizzaToppingsCmd.Parameters.AddWithValue("@pizzaId", pizzaId);
                            pizzaToppingsCmd.Parameters.AddWithValue("@toppingId", toppingId);
                            pizzaToppingsCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Did not enter the data correctly. {e.Message}");
            }
            return cartDto;
        }

        public CartDto GetSidesOrder(CartDto cartDto, int orderId, int sideQuantity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (Sides items in cartDto.Sides)
                    {
                        string sidesSql = "INSERT INTO Sides (sidesName, sideQuantity, availability) VALUES(@sidesName, @sideQuantity, @availability);" +
                        "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                        SqlCommand sidesCmd = new SqlCommand(sidesSql, connection);
                        sidesCmd.Parameters.AddWithValue("@sidesName", items.Name);
                        sidesCmd.Parameters.AddWithValue("@sideQuantity", sideQuantity);
                        sidesCmd.Parameters.AddWithValue("@availability", true);
                        int sideId = (int)sidesCmd.ExecuteScalar();

                        // Insert into Side_order table
                        string sideOrderSql = "INSERT INTO Sides_Order (sideId, orderId) VALUES(@sideId, @orderId);";
                        SqlCommand sideOrderCmd = new SqlCommand(sideOrderSql, connection);
                        sideOrderCmd.Parameters.AddWithValue("@sideId", sideId);
                        sideOrderCmd.Parameters.AddWithValue("@orderId", orderId);
                        sideOrderCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Did not enter the data correctly. {e.Message}");
            }
            return cartDto;
        }
        public List<SpecialtyPizza> GetSpecialtyPizzasByNames(List<string> names)
        {
            List<SpecialtyPizza> specialtyPizzas = new List<SpecialtyPizza>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("", conn); // Create an empty SqlCommand object

                    // Build the SQL query with the IN clause and dynamically add the parameter values
                    string query = $"SELECT c.createSpecialtyPizzaId, c.name, description, isAvailable, size, sauces, crusts, c.price, STRING_AGG(t.name, ',') AS toppings, imageUrl " +
                        $"FROM CreateSpecialtyPizzas c " +
                        $"JOIN CreatePizzaToppings ct ON ct.createPizzaId = c.createSpecialtyPizzaId " +
                        $"JOIN CreateToppings t ON t.toppingId = ct.toppingId " +
                        $"WHERE c.name IN ({string.Join(",", names.Select((name, index) => $"@name{index}"))}) " +
                        $"GROUP BY c.createSpecialtyPizzaId, c.name, description, isAvailable, size, sauces, crusts, c.price, imageUrl " +
                        $"ORDER BY c.name;";

                    cmd.CommandText = query; // Set the SQL query to the SqlCommand object

                    // Add parameters for each name in the names list
                    for (int i = 0; i < names.Count; i++)
                    {
                        cmd.Parameters.AddWithValue($"@name{i}", names[i]);
                    }

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SpecialtyPizza newSpecialtyPizza = CreateSpecialtyPizzaFromReader(reader);
                        specialtyPizzas.Add(newSpecialtyPizza);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return specialtyPizzas;
        }


        public List<ItemNames> GetSidesByNames(List<string> names)
        {
            List<ItemNames> sides = new List<ItemNames>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("", conn);

                    string query = $"SELECT sideName, sidePrice " +
                                   $"FROM CreateSides " +
                                   $"WHERE sideName IN ({string.Join(",", names.Select(name => "'" + name + "'"))});";

                    cmd.CommandText = query;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ItemNames newSides = CreateSideNameFromReader(reader);
                        sides.Add(newSides);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return sides;
        }

        public List<ItemNames> GetBeveragesByNames(List<string> names)
        {
            List<ItemNames> beverages = new List<ItemNames>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("", conn);

                    string query = $"SELECT beverageName, beveragePrice " +
                                   $"FROM CreateBeverage " +
                                   $"WHERE beverageName IN ({string.Join(",", names.Select(name => "'" + name + "'"))});";

                    cmd.CommandText = query;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ItemNames newBeverages = CreateBeverageNameFromReader(reader);
                        beverages.Add(newBeverages);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return beverages;
        }

        public CartDto GetBeverageOrder(CartDto cartDto, int orderId, int beverageQuantity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (Beverages items in cartDto.Beverages)
                    {
                        string beverageSql = "INSERT INTO Beverage (beverageName, beverageQuantity, availability) VALUES(@beverageName, @beverageQuantity, @availability);" +
                        "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                        SqlCommand beverageCmd = new SqlCommand(beverageSql, connection);
                        beverageCmd.Parameters.AddWithValue("@beverageName", items.Name);
                        beverageCmd.Parameters.AddWithValue("@beverageQuantity", beverageQuantity);
                        beverageCmd.Parameters.AddWithValue("@availability", true);
                        int beverageId = (int)beverageCmd.ExecuteScalar();

                        // Insert into Order_beverage table
                        string beverageOrderSql = "INSERT INTO Order_Beverage (beverageid, orderid) VALUES(@beverageid, @orderid);";
                        SqlCommand beverageOrderCmd = new SqlCommand(beverageOrderSql, connection);
                        beverageOrderCmd.Parameters.AddWithValue("@beverageid", beverageId);
                        beverageOrderCmd.Parameters.AddWithValue("@orderid", orderId);
                        beverageOrderCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Did not enter the data correctly. {e.Message}");
            }
            return cartDto;
        }

        public CartDto GetSpecialtyOrder(CartDto cartDto, int orderId, int specialtyQuantity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (SpecialtyPizzaCart items in cartDto.SpecialtyPizzas)
                    {
                        string specialtySql = "INSERT INTO Specialty (specialtyName, specialtyQuantity, availability) VALUES(@specialtyName, @specialtyQuantity, @availability);" +
                        "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                        SqlCommand beverageCmd = new SqlCommand(specialtySql, connection);
                        beverageCmd.Parameters.AddWithValue("@specialtyName", items.Name);
                        beverageCmd.Parameters.AddWithValue("@specialtyQuantity", specialtyQuantity);
                        beverageCmd.Parameters.AddWithValue("@availability", true);
                        int specialtyId = (int)beverageCmd.ExecuteScalar();

                        // Insert into Specialty_Order table
                        string specialtyOrderSql = "INSERT INTO Specialty_Order (specialtyId, orderid) VALUES(@specialtyId, @orderid);";
                        SqlCommand specialtyOrderCmd = new SqlCommand(specialtyOrderSql, connection);
                        specialtyOrderCmd.Parameters.AddWithValue("@specialtyId", specialtyId);
                        specialtyOrderCmd.Parameters.AddWithValue("@orderid", orderId);
                        specialtyOrderCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Did not enter the data correctly. {e.Message}");
            }
            return cartDto;
        }

        private SpecialtyPizza CreateSpecialtyPizzaFromReader(SqlDataReader reader)
        {
            SpecialtyPizza specialtyPizza = new SpecialtyPizza();
            specialtyPizza.CreatePizzaId = Convert.ToInt32(reader["createSpecialtyPizzaId"]);
            specialtyPizza.Name = Convert.ToString(reader["name"]);
            specialtyPizza.Description = Convert.ToString(reader["description"]);
            specialtyPizza.IsAvailable = Convert.ToBoolean(reader["isAvailable"]);
            specialtyPizza.PizzaSize = Convert.ToString(reader["size"]);
            specialtyPizza.PizzaCrust = Convert.ToString(reader["crusts"]);
            specialtyPizza.PizzaSauce = Convert.ToString(reader["sauces"]);
            specialtyPizza.PizzaToppings = Convert.ToString(reader["toppings"]);
            specialtyPizza._Price = Convert.ToDecimal(reader["price"]);
            specialtyPizza.ImageUrl = Convert.ToString(reader["imageUrl"]);
            return specialtyPizza;
        }

        private ItemNames CreateBeverageNameFromReader(SqlDataReader reader)
        {
            ItemNames items = new ItemNames();
            items.Name = Convert.ToString(reader["beverageName"]);
            items.Price = Convert.ToInt32(reader["beveragePrice"]);

            return items;
        }
        private ItemNames CreateSideNameFromReader(SqlDataReader reader)
        {
            ItemNames items = new ItemNames();
            items.Name = Convert.ToString(reader["sideName"]);
            items.Price = Convert.ToInt32(reader["sidePrice"]);

            return items;
        }
    }
}
