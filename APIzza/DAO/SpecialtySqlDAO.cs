using APIzza.Models;
using Microsoft.Data.SqlClient;

namespace APIzza.DAO
{
    public class SpecialtySqlDAO : ISpecialtyDAO
    {
        private readonly string connectionString;
        public SpecialtySqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        //Get the list of all specialty pizzas
        public IList<SpecialtyPizza> GetAllAvailableSpecialtyPizza()
        {
            IList<SpecialtyPizza> result = new List<SpecialtyPizza>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT c.createSpecialtyPizzaId, c.name, description, isAvailable, size, sauces, crusts, c.price, imageUrl, STRING_AGG(t.name, ',') AS toppings " +
                        "FROM CreateSpecialtyPizzas c " +
                        "JOIN CreatePizzaToppings ct ON ct.createPizzaId = c.createSpecialtyPizzaId " +
                        "JOIN CreateToppings t ON t.toppingId = ct.toppingId WHERE isAvailable = 1 " +
                        "GROUP BY c.createSpecialtyPizzaId, c.name, description, isAvailable, size, sauces, crusts, c.price, imageUrl " +
                        "ORDER BY c.createSpecialtyPizzaId;";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        SpecialtyPizza specialtyPizza = new SpecialtyPizza();
                        specialtyPizza = CreateSpecialtyPizzaFromReader(reader);
                        result.Add(specialtyPizza);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Unable to retrieve to data {e.Message}");
            }
            return result;
        }

        public List<SpecialtyPizza> GetSpecialtyPizzasByIds(List<int> ids)
        {
            List<SpecialtyPizza> specialtyPizzas = new List<SpecialtyPizza>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("", conn); // Create an empty SqlCommand object

                    // Build the SQL query with the IN clause and dynamically add the parameter values
                    string query = $"SELECT c.createSpecialtyPizzaId, c.name, description, isAvailable, size, sauces, crusts, c.price, imageUrl, STRING_AGG(t.name, ',') AS toppings " +
                                   $"FROM CreateSpecialtyPizzas c " +
                                   $"JOIN CreatePizzaToppings ct ON ct.createPizzaId = c.createSpecialtyPizzaId " +
                                   $"JOIN CreateToppings t ON t.toppingId = ct.toppingId " +
                                   $"WHERE c.createSpecialtyPizzaId IN ({string.Join(",", ids)}) " +
                                   $"GROUP BY c.createSpecialtyPizzaId, c.name, description, isAvailable, size, sauces, crusts, c.price, imageUrl " +
                                   $"ORDER BY c.name ";
                    cmd.CommandText = query; // Set the SQL query to the SqlCommand object

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
