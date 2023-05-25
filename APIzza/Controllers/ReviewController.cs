using APIzza.DAO;
using APIzza.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIzza.Controllers
{
    [Route("api/review")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private IReviewDao reviewDao;

        public ReviewController(IReviewDao _reviewDao)
        {
            this.reviewDao = _reviewDao;
        }

        [HttpGet]
        public ActionResult<Review> DisplayReview()
        {
            return Ok(reviewDao.DisplayReview());
        }

        [HttpPost]
        public ActionResult<Review> GetReviews(Review review)
        {
            reviewDao.GetReview(review);

            return Created("/api/review/" + review.Review_Id, review);
        }
    }
}
