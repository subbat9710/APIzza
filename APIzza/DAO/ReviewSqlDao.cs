using APIzza.Models;
using Microsoft.Data.SqlClient;

namespace APIzza.DAO
{
    public class ReviewSqlDao : IReviewDao
    {
        private readonly string connectionString;

        public ReviewSqlDao(string dbconnectionString)
        {
            this.connectionString = dbconnectionString;
        }

        public Review GetReview(Review review)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Insert reviews
                    string reviewSql = "INSERT INTO [review] (rating, name, comment, created_at) " +
                        "VALUES(@rating, @name, @comment, @created_at); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS INT);";
                    SqlCommand reviewCmd = new SqlCommand();
                    reviewCmd.CommandText = reviewSql;
                    reviewCmd.Connection = connection;

                    reviewCmd.Parameters.AddWithValue("@rating", review.Rating);
                    reviewCmd.Parameters.AddWithValue("@name", review.Name);
                    reviewCmd.Parameters.AddWithValue("@comment", review.Comment);
                    reviewCmd.Parameters.AddWithValue("@created_at", DateTime.Now);

                    int review_id = (int)reviewCmd.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Did not enter the data correctly. {e.Message}");
            }
            return review;
        }

        public List<Review> DisplayReview()
        {
            List<Review> result = new List<Review>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Insert reviews
                    string reviewSql = "SELECT rating, name, comment, created_at FROM review " +
                        "ORDER BY review_id DESC;";
                    SqlCommand reviewCmd = new SqlCommand();
                    reviewCmd.CommandText = reviewSql;
                    reviewCmd.Connection = connection;

                    SqlDataReader reader = reviewCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Review newReview = CreateReviewFromReader(reader);
                        result.Add(newReview);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Did not enter the data correctly. {e.Message}");
            }
            return result;
        }

        public Review CreateReviewFromReader(SqlDataReader reader)
        {
            Review reviews = new Review();
            // reviews.Review_Id = Convert.ToInt32(reader["review_id"]);
            reviews.Rating = Convert.ToInt32(reader["rating"]);
            reviews.Name = Convert.ToString(reader["name"]);
            reviews.Comment = Convert.ToString(reader["comment"]);
            reviews.Created = Convert.ToDateTime(reader["created_at"]);

            return reviews;
        }
    }
}
