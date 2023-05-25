using APIzza.DTO;
using APIzza.Models;
using Microsoft.Data.SqlClient;

namespace APIzza.DAO
{
    public class PendingOrdersSqlDao : IPendingOrders
    {
        private readonly string connectionString;

        public PendingOrdersSqlDao(string dbconnectionString)
        {
            this.connectionString = dbconnectionString;
        }

        public IList<PendingOrders> ViewPendingOrders()
        {
            IList<PendingOrders> result = new List<PendingOrders>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT op.orderId, ordercost, orderStatus, phoneNumber, email, ordertype, orderDate, " +
                        "COALESCE(c.crustName, '-') AS crustName, COALESCE(s.sauceName, '-') AS sauceName, " +
                        "COALESCE(size.size, '-') AS size, COALESCE(STRING_AGG(t.toppingName, ', '), '-') AS toppings, " +
                        "COALESCE(si.sidesName, '-') AS sidesName, ISNULL(si.sideQuantity, 0) AS sideQuantity, " +
                        "COALESCE(b.BeverageName, '-') AS BeverageName, ISNULL(b.beverageQuantity, 0) AS beverageQuantity, " +
                        "COALESCE(sp.specialtyName, '-') AS specialtyName, ISNULL(sp.specialtyQuantity, 0) AS specialtyQuantity " +
                        "FROM OrderPizza op " +
                        "LEFT JOIN Pizza p ON p.orderId = op.orderId " +
                        "LEFT JOIN Crust c ON c.crustId = p.crustId " +
                        "LEFT JOIN Sauce s ON s.sauceId = p.sauceId " +
                        "LEFT JOIN Size size ON size.sizeId = p.sizeId " +
                        "LEFT JOIN PizzaTopping pt ON pt.pizzaId = p.pizzaId " +
                        "LEFT JOIN Topping t ON t.toppingId = pt.toppingId " +
                        "LEFT JOIN Sides_Order so ON so.orderId = op.orderId " +
                        "LEFT JOIN Sides si ON si.sideId = so.sideId " +
                        "LEFT JOIN Order_Beverage ob ON ob.orderId = op.orderId " +
                        "LEFT JOIN Beverage b ON b.beverageId = ob.beverageid " +
                        "LEFT JOIN Specialty_Order spo ON spo.orderId = op.orderId " +
                        "LEFT JOIN Specialty sp ON sp.specialtyId = spo.specialtyId " +
                        "GROUP BY op.orderId, ordercost, orderStatus, ordertype, phoneNumber, email, orderDate, c.crustName, s.sauceName, size.size, si.sidesName, si.sideQuantity, b.BeverageName, b.beverageQuantity, sp.specialtyName, sp.specialtyQuantity ORDER BY op.orderId;";
                    cmd.CommandText = sql;
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PendingOrders pendingOrders = new PendingOrders();
                        pendingOrders = CreatePendingOrdersFromReader(reader);
                        result.Add(pendingOrders);
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to view the pendingOrders {e.Message}");
            }
            return result;
        }

        public IList<PendingOrders> ViewHistoricalReport(int employeeId)
        {
            IList<PendingOrders> result = new List<PendingOrders>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT e.username, op.orderId, op.orderDate, op.orderStatus, op.ordertype, op.ordercost " +
                        "FROM OrderPizza op JOIN Employee e ON e.user_id = op.employeeId " +
                        "WHERE op.orderStatus = 'Completed';";
                    cmd.CommandText = sql;
                    cmd.Connection = connection;
                    //cmd.Parameters.AddWithValue("@user_id", employeeId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PendingOrders pendingOrders = new PendingOrders();
                        pendingOrders = CreateHistoricalReportFromReader(reader);
                        result.Add(pendingOrders);
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to view the pendingOrders {e.Message}");
            }
            return result;
        }
        public IList<PendingOrders> GetPizzaById(int id)
        {
            IList<PendingOrders> result = new List<PendingOrders>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT op.orderId, ordercost, orderStatus, ordertype, orderDate, c.crustName, " +
                        "s.sauceName, size.size, STRING_AGG(t.toppingName, ', ') AS toppings FROM OrderPizza op " +
                        "JOIN Pizza p ON p.orderId = op.orderId " +
                        "JOIN Crust c ON c.crustId = p.crustId " +
                        "JOIN Sauce s ON s.sauceId = p.sauceId " +
                        "JOIN Size size ON size.sizeId = p.sizeId " +
                        "JOIN PizzaTopping pt ON pt.pizzaId = p.pizzaId " +
                        "JOIN Topping t ON t.toppingId = pt.toppingId " +
                        "where op.orderId = @param " +
                        "GROUP BY op.orderId, ordercost, orderStatus, ordertype, orderDate, c.crustName, s.sauceName, size.size ORDER BY op.orderId";

                    cmd.CommandText = sql;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@param", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PendingOrders pendingOrders = new PendingOrders();
                        pendingOrders = CreatePendingOrdersFromReader(reader);
                        result.Add(pendingOrders);
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to view the pendingOrders {e.Message}");
            }
            return result;
        }

        public IList<SalesDto> SalesReport()
        {
            IList<SalesDto> result = new List<SalesDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT orderDate, SUM(CASE WHEN specialtyName IS NOT NULL THEN ordercost ELSE 0 END) AS specialty_sales, " +
                        "SUM(CASE WHEN sidesName IS NOT NULL THEN ordercost ELSE 0 END) AS sides_sales, " +
                        "SUM(CASE WHEN BeverageName IS NOT NULL THEN ordercost ELSE 0 END) AS beverage_sales, op.ordertype " +
                        "FROM OrderPizza op " +
                        "LEFT JOIN Pizza p ON p.orderId = op.orderId " +
                        "LEFT JOIN PizzaTopping pt ON pt.pizzaId = p.pizzaId " +
                        "LEFT JOIN Sides_Order so ON so.orderId = op.orderId " +
                        "LEFT JOIN Sides si ON si.sideId = so.sideId " +
                        "LEFT JOIN Order_Beverage ob ON ob.orderId = op.orderId " +
                        "LEFT JOIN Beverage b ON b.beverageId = ob.beverageid " +
                        "LEFT JOIN Specialty_Order spo ON spo.orderId = op.orderId " +
                        "LEFT JOIN Specialty sp ON sp.specialtyId = spo.specialtyId " +
                        "GROUP BY orderDate, op.ordertype " +
                        "ORDER BY op.orderDate DESC;";

                    cmd.CommandText = sql;
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        SalesDto sales = new SalesDto();
                        sales = CreateSalesReportFromReader(reader);
                        result.Add(sales);
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to view the pendingOrders {e.Message}");
            }
            return result;
        }


        private PendingOrders CreatePendingOrdersFromReader(SqlDataReader reader)
        {
            PendingOrders pendingOrders = new PendingOrders();
            pendingOrders.OrderId = Convert.ToInt32(reader["orderId"]);
            pendingOrders.OrderStatus = Convert.ToString(reader["orderStatus"]);
            pendingOrders.OrderType = Convert.ToString(reader["orderType"]);
            pendingOrders.OrderTime = Convert.ToDateTime(reader["orderDate"]);
            pendingOrders.OrderCost = Convert.ToDecimal(reader["ordercost"]);
            pendingOrders.Toppings = Convert.ToString(reader["toppings"]);
            pendingOrders.Size = Convert.ToString(reader["size"]);
            pendingOrders.CrustName = Convert.ToString(reader["crustName"]);
            pendingOrders.SauceName = Convert.ToString(reader["sauceName"]);
            pendingOrders.Email = Convert.ToString(reader["email"]);
            pendingOrders.PhoneNumber = Convert.ToString(reader["phoneNumber"]);
            pendingOrders.SideName = Convert.ToString(reader["sidesName"]);
            pendingOrders.BeverageName = Convert.ToString(reader["BeverageName"]);
            pendingOrders.SpecialtyName = Convert.ToString(reader["SpecialtyName"]);
            pendingOrders.sideQuantity = Convert.ToInt32(reader["sideQuantity"]);
            pendingOrders.beverageQuantity = Convert.ToInt32(reader["beverageQuantity"]);
            pendingOrders.specialtyQuantity = Convert.ToInt32(reader["specialtyQuantity"]);

            return pendingOrders;
        }

        private PendingOrders CreateHistoricalReportFromReader(SqlDataReader reader)
        {
            PendingOrders pendingOrders = new PendingOrders();
            pendingOrders.OrderId = Convert.ToInt32(reader["orderId"]);
            pendingOrders.OrderStatus = Convert.ToString(reader["orderStatus"]);
            pendingOrders.OrderType = Convert.ToString(reader["orderType"]);
            pendingOrders.OrderTime = Convert.ToDateTime(reader["orderDate"]);
            pendingOrders.OrderCost = Convert.ToDecimal(reader["ordercost"]);
            pendingOrders.Username = Convert.ToString(reader["username"]);

            return pendingOrders;
        }

        private SalesDto CreateSalesReportFromReader(SqlDataReader reader)
        {
            SalesDto newSales = new SalesDto();
            newSales.OrderDate = Convert.ToString(reader["orderDate"]);
            newSales.SidesSale = Convert.ToDecimal(reader["sides_sales"]);
            newSales.BeverageSale = Convert.ToDecimal(reader["beverage_sales"]);
            newSales.SpecialtySale = Convert.ToDecimal(reader["specialty_sales"]);
            newSales.OrderType = Convert.ToString(reader["orderType"]);

            return newSales;
        }
    }
}
