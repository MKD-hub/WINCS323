using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebProject.Data;
using WebProject.Model;
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

        [HttpGet("get-id/{reviewId}")]
        public async Task<IActionResult> GetReviewById([FromRoute] int reviewId)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            return Ok(review);
        }

        
        [HttpPost("add-review")]
        [Authorize]
        public async Task<IActionResult> AddReview([FromForm] ReviewModel reviewModel)
        {

            var revId = await _reviewRepository.AddReviewAsync(reviewModel);

            return CreatedAtAction(nameof(GetReviewById), new { reviewId = revId, controller = "reviews" }, revId);
            
        }

        
        [HttpPut("edit-review/{reviewId}")]
        [Authorize]
        public async Task<IActionResult> EditReview([FromRoute] int reviewId, [FromForm] ReviewModel reviewModel)
        {
            await _reviewRepository.EditReviewAsync(reviewId, reviewModel);
            return Ok();
        }

        [HttpDelete("remove-review/{reviewId}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview([FromRoute] int reviewId)
        {
            await _reviewRepository.DeleteReviewAsync(reviewId);
            return Ok();
        }
        
       
    }
}
