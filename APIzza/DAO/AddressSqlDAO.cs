using APIzza.Models;
using Microsoft.Data.SqlClient;

namespace APIzza.DAO
{
    public class AddressSqlDAO : IAddressDAO
    {
        private readonly string connectionString;

        public AddressSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public Address GetAddress(string addressID)
        {
            Address returnAddress = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM address WHERE addressId = @address_id ;", conn);
                    cmd.Parameters.AddWithValue("@address_id", addressID);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        returnAddress = GetAddressFromReader(reader);
                    }
                }
                return returnAddress;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public Address AddAddress(Address addMe)
        {



            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();
                    if (addMe.Apartment != null && addMe.Apartment != "")
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO address (homeType, streetAddress, apartmentNumber, city, state, zipCode, instructions) " +
                            "VALUES (@homeType, @street_address, @apartment, @city, @state, @zip, @instructions);", conn);
                        cmd.Parameters.AddWithValue("@homeType", addMe.HomeType);
                        cmd.Parameters.AddWithValue("@street_address", addMe.StreetAddress);
                        cmd.Parameters.AddWithValue("@apartment", addMe.Apartment);
                        cmd.Parameters.AddWithValue("@city", addMe.City);
                        cmd.Parameters.AddWithValue("@state", addMe.StateAbbreviation);
                        cmd.Parameters.AddWithValue("@zip", addMe.Zip);
                        cmd.Parameters.AddWithValue("@instructions", addMe.Instructions);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = ("SELECT * FROM address WHERE streetAddress = @street_address2 " +
                                                "AND apartmentNumber = @apartment2 AND city = @city2 AND zipCode = @zip2 ;");
                        cmd.Parameters.AddWithValue("@street_address2", addMe.StreetAddress);
                        cmd.Parameters.AddWithValue("@apartment2", addMe.Apartment);
                        cmd.Parameters.AddWithValue("@city2", addMe.City);
                        cmd.Parameters.AddWithValue("@zip2", addMe.Zip);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            Address inserted;
                            inserted = GetAddressFromReader(reader);
                            return inserted;
                        }
                        else
                        {
                            return null;
                        }

                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO address (homeType, streetAddress, city, state, zipCode, instructions) " +
                            "VALUES (@homeType, @street_address, @city, @state, @zip, @instructions);", conn);
                        cmd.Parameters.AddWithValue("@homeType", addMe.HomeType);
                        cmd.Parameters.AddWithValue("@street_address", addMe.StreetAddress);
                        cmd.Parameters.AddWithValue("@city", addMe.City);
                        cmd.Parameters.AddWithValue("@state", addMe.StateAbbreviation);
                        cmd.Parameters.AddWithValue("@zip", addMe.Zip);
                        cmd.Parameters.AddWithValue("@instructions", addMe.Instructions);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = ("SELECT * from address WHERE streetAddress = @street_address2 " +
                                                "AND city = @city2 AND zipCode = @zip2 ;");
                        cmd.Parameters.AddWithValue("@street_address2", addMe.StreetAddress);
                        cmd.Parameters.AddWithValue("@city2", addMe.City);
                        cmd.Parameters.AddWithValue("@zip2", addMe.Zip);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            Address inserted;
                            inserted = GetAddressFromReader(reader);
                            return inserted;
                        }
                        else
                        {
                            return null;
                        }

                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public Address EditAddress(Address update)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE address SET streetAddress = @street_address, apartmentNumber = @apartment, city = @city, state = @state, zipCode = @zip, instructions = @instructions " +
                        "WHERE addressId = @address_id ;", conn);
                    cmd.Parameters.AddWithValue("@address_id", update.AddressID);
                    cmd.Parameters.AddWithValue("@street_address", update.StreetAddress);
                    cmd.Parameters.AddWithValue("@apartment", update.Apartment);
                    cmd.Parameters.AddWithValue("@city", update.City);
                    cmd.Parameters.AddWithValue("@state", update.StateAbbreviation);
                    cmd.Parameters.AddWithValue("@zip", update.Zip);
                    cmd.Parameters.AddWithValue("@instructions", update.Instructions);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT * from address WHERE addressId = @address_id2 ;";
                    cmd.Parameters.AddWithValue("@address_id2", update.AddressID);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return GetAddressFromReader(reader);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Address GetAddressFromReader(SqlDataReader reader)
        {
            Address a = new Address()
            {
                HomeType = Convert.ToString(reader["homeType"]),
                StreetAddress = Convert.ToString(reader["streetAddress"]),
                Apartment = Convert.ToString(reader["apartmentNumber"]),
                City = Convert.ToString(reader["city"]),
                StateAbbreviation = Convert.ToString(reader["state"]),
                Zip = Convert.ToString(reader["zipCode"]),
                AddressID = Convert.ToInt32(reader["addressId"]),
                Instructions = Convert.ToString(reader["instructions"]),
            };
            return a;
        }
    }
}
