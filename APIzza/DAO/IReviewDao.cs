using APIzza.Models;

namespace APIzza.DAO
{
    public interface IReviewDao
    {
        Review GetReview(Review review);
        List<Review> DisplayReview();
    }
}
