using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using GreenWashAPI.VMClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenWashAPI.Controllers
{ 
        [ApiController]
        [Route("api/[controller]")]
  
        public class ReviewsController : ControllerBase
        {
            private readonly IReviews _reviewRepo;
            public ReviewsController(IReviews reviewRepo)
            {
                _reviewRepo = reviewRepo;
            }

            // Creates a new review with a rating and an allowed comment.
            [HttpPost("add")]
            [Authorize(Policy = "AdminCustomerPolicy")]
            public async Task<IActionResult> CreateReview([FromBody] ReviewVM review)
            {
            Console.WriteLine($"Received Review: Rating: {review.Rating}, Comment: {review.Comment}");
            Review reviewObj = new Review()
                 {
                     Rating = review.Rating,
                     Comment = review.Comment
                 };


                try
                {
                    var createdReview = await _reviewRepo.CreateReviewAsync(reviewObj);
               
                return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(new { error = ex.Message });
                }
            }

            // Retrieves a review by its ID.
            [HttpGet("{reviewId}")]
            [Authorize(Policy = "AdminPolicy")]
            public async Task<IActionResult> GetReviewById(int reviewId)
            {
                try
                {
                    var review = await _reviewRepo.GetReviewByIdAsync(reviewId);
                    if (review == null)
                        return NotFound(new { error = "Review not found." });
                    return Ok(review);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { error = ex.Message });
                }
            }

            // Retrieves the average rating for a specific washer.
            [HttpGet("average_washer/{washerId}")]
            [Authorize(Policy = "AdminPolicy")]  
            public async Task<IActionResult> GetAverageRatingForWasher(int washerId)
            {
                try
                {
                    double avgRating = await _reviewRepo.GetAverageRatingForWasherAsync(washerId);
                    return Ok(new { washerId, averageRating = avgRating });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { error = ex.Message });
                }
            }

            // Adds a comment to an existing review if no comment exists.
            [HttpPost("{reviewId}/add-comment")]
            [Authorize(Policy = "AdminPolicy")]
            public async Task<IActionResult> AddCommentForReview(int reviewId, [FromBody] string comment)
            {
                try
                {
                    var review = await _reviewRepo.AddCommentForReviewAsync(reviewId, comment);
                    return Ok(review);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { error = ex.Message });
                }
            }


            // Updates the comment for an existing review.
            [HttpPut("{reviewId}/update-comment")]
            [Authorize(Policy = "AdminPolicy")]
            public async Task<IActionResult> UpdateCommentForReview(int reviewId, [FromBody] string newComment)
            {
                try
                {
                    var review = await _reviewRepo.UpdateCommentForReviewAsync(reviewId, newComment);
                    return Ok(review);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { error = ex.Message });
                }
            }

           
            [HttpDelete("{reviewId}/delete-comment")]
            [Authorize(Policy = "AdminPolicy")]
            public async Task<IActionResult> DeleteCommentForReview(int reviewId)
            {
                try
                {
                    var review = await _reviewRepo.DeleteCommentForReviewAsync(reviewId);
                    return Ok(review);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { error = ex.Message });
                }
            }

        // Endpoint to get WasherId by OrderId
        [HttpGet("GetWasherId/{orderId}")]
        public async Task<ActionResult<int?>> GetWasherId(int orderId)
        {
            var washerId = await _reviewRepo.GetWasherIdByOrderIdAsync(orderId);

            if (washerId == null)
            {
                return NotFound("Washer not found for this OrderId.");
            }

            return Ok(washerId);
        }
    }
}

