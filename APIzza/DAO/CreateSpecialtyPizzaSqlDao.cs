using APIzza.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APIzza.DAO
{
    public class CreateSpecialtyPizzaSqlDao : ICreateSpecialtyPizaa
    {
        private readonly string connectionString;

        public CreateSpecialtyPizzaSqlDao(string dbconnectionString)
        {
            this.connectionString = dbconnectionString;
        }
        public CreateSpecialtyPizza CreateSpecialtyPizza(CreateSpecialtyPizza createSpecialty, int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    // Insert pizza
                    string createSpecialtyPizzaSql = "INSERT INTO CreateSpecialtyPizzas (name, description, isAvailable, size, sauces, crusts, price, employeeId, imageUrl) " +
                        "VALUES (@name, @description, @isAvailable, @size, @sauces, @crusts, @price, @employeeId, @imageUrl);" +
                                       "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                    SqlCommand createSpecialtyPizza = new SqlCommand(createSpecialtyPizzaSql, connection);
                    createSpecialtyPizza.Parameters.AddWithValue("@name", createSpecialty.Name);
                    createSpecialtyPizza.Parameters.AddWithValue("@description", createSpecialty.Description);
                    createSpecialtyPizza.Parameters.AddWithValue("@isAvailable", true);
                    createSpecialtyPizza.Parameters.AddWithValue("@size", createSpecialty.Size.Size.ToString());
                    createSpecialtyPizza.Parameters.AddWithValue("@sauces", createSpecialty.Sauces.Sauces.ToString());
                    createSpecialtyPizza.Parameters.AddWithValue("@crusts", createSpecialty.Crusts.Crusts.ToString());
                    createSpecialtyPizza.Parameters.AddWithValue("@price", createSpecialty.Price);
                    createSpecialtyPizza.Parameters.AddWithValue("@imageUrl", createSpecialty.ImageUrl);
                    createSpecialtyPizza.Parameters.AddWithValue("@employeeId", userId);
                    int createSpecialtyPizzaId = (int)createSpecialtyPizza.ExecuteScalar();

                    createSpecialty.PizzaId = createSpecialtyPizzaId;

                    // Insert toppings
                    foreach (PizzaTopping topping in createSpecialty.Toppings)
                    {
                        string toppingSql = "INSERT INTO CreateToppings (name, price) VALUES (@name, @price); " +
                                            "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                        SqlCommand toppingCmd = new SqlCommand(toppingSql, connection);
                        toppingCmd.Parameters.AddWithValue("@name", topping.Topping.ToString());
                        toppingCmd.Parameters.AddWithValue("@price", topping.Price);
                        int toppingId = (int)toppingCmd.ExecuteScalar();

                        // Insert into PizzaToppings table
                        string pizzaToppingsSql = "INSERT INTO CreatePizzaToppings (createPizzaId, toppingId) VALUES (@createPizzaId, @toppingId);";
                        SqlCommand pizzaToppingsCmd = new SqlCommand(pizzaToppingsSql, connection);
                        pizzaToppingsCmd.Parameters.AddWithValue("@createPizzaId", createSpecialtyPizzaId);
                        pizzaToppingsCmd.Parameters.AddWithValue("@toppingId", toppingId);
                        pizzaToppingsCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Did not enter the data correctly. {e.Message}");
            }
            return createSpecialty;
        }

        //Employee can able to show the Specialty Pizza to customers as per the availability
        public bool ChangeAvailability(int createId, bool isAvailable)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE CreateSpecialtyPizzas SET isAvailable = @isAvailable WHERE createSpecialtyPizzaId = @createSpecialtyPizzaId;";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;

                    cmd.Parameters.AddWithValue("@isAvailable", isAvailable);
                    cmd.Parameters.AddWithValue("@createSpecialtyPizzaId", createId);

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

        public bool EditSpecialtyPizza(int createId, CreateSpecialtyPizza specialtyPizza)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE CreateSpecialtyPizzas SET name = @name, description = @description, size = @size, sauces = @sauces, " +
                        "crusts = @crusts, price = @price, imageUrl = @imageUrl WHERE createSpecialtyPizzaId = @createSpecialtyPizzaId;";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;

                    cmd.Parameters.AddWithValue("@name", specialtyPizza.Name);
                    cmd.Parameters.AddWithValue("@description", specialtyPizza.Description);
                    cmd.Parameters.AddWithValue("@size", specialtyPizza.Size.Size);
                    cmd.Parameters.AddWithValue("@sauces", specialtyPizza.Sauces.Sauces);
                    cmd.Parameters.AddWithValue("@crusts", specialtyPizza.Crusts.Crusts);
                    cmd.Parameters.AddWithValue("@price", specialtyPizza.Price);
                    cmd.Parameters.AddWithValue("@imageUrl", specialtyPizza.ImageUrl);
                    cmd.Parameters.AddWithValue("@createSpecialtyPizzaId", createId);

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

        public bool RemoveSpecialtyPizza(int createId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create the command object
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "RemoveSpecialtyPizza";

                    // Add the parameters to the command object
                    cmd.Parameters.AddWithValue("@createSpecialtyPizzaId", createId);

                    // Execute the command and return the result
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected == 1;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public SpecialtyPizza GetSpecialtyPizzaById(int createId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT c.createSpecialtyPizzaId, c.name, description, isAvailable, size, sauces, crusts, c.price, imageUrl, STRING_AGG(t.name, ',') AS toppings " +
                        "FROM CreateSpecialtyPizzas c " +
                        "JOIN CreatePizzaToppings ct ON ct.createPizzaId = c.createSpecialtyPizzaId " +
                        "JOIN CreateToppings t ON t.toppingId = ct.toppingId " +
                        "WHERE isAvailable = 1  AND c.createSpecialtyPizzaId = @createSpecialtyPizzaId " +
                        "GROUP BY c.createSpecialtyPizzaId, c.name, description, isAvailable, size, sauces, crusts, c.price, imageUrl " +
                        "ORDER BY c.createSpecialtyPizzaId";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;

                    cmd.Parameters.AddWithValue("@createSpecialtyPizzaId", createId);

                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        SpecialtyPizza result = CreateSpecialtyPizzaFromReader(reader);
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
    }
}
