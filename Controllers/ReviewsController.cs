using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProject.Repository;

namespace WebProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        /*Here is where all the API's will go*/
        private readonly IReviewsRepository _reviewRepository;
        public ReviewsController(IReviewsRepository reviewsRepository)
        {
            _reviewRepository = reviewsRepository;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews =  await _reviewRepository.GetAllReviewsAsync();
            return Ok(reviews);
        }

        [HttpGet("getId/{id}")]
        public async Task<IActionResult> GetReviewById([FromRoute] int reviewId)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            return Ok(review);
        }
    }
}
