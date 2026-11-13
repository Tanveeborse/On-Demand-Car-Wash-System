using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenWashAPI.Repository
{
    public class ReviewRepo:IReviews
    {
        private readonly GreenWashDbContext _context;
        // Allowed comments for review ratings.
        private readonly string[] allowedComments = { "Excellent", "Very Good", "Good", "Average", "Poor" };
        public ReviewRepo(GreenWashDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Creates a new review with rating and comment.
        /// The rating must be between 1 and 5 and the comment must be one of the allowed options.
        /// </summary>
        public async Task<Review> CreateReviewAsync(Review review)
        {
            // Validate rating value.
            if (review.Rating < 1 || review.Rating > 5)
                throw new Exception("Rating must be between 1 and 5.");
            // Validate comment.
            if (string.IsNullOrEmpty(review.Comment) || !allowedComments.Contains(review.Comment))
            {
                throw new Exception("Invalid comment selected. Allowed comments are: " + string.Join(", ", allowedComments));
            }
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }
        /// <summary>
        /// Retrieves a review by its ID.
        /// </summary>
        public async Task<Review> GetReviewByIdAsync(int reviewId)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }
        // Calculate the average rating for a specific washer.
        public async Task<double> GetAverageRatingForWasherAsync(int washerId)
        {
            var ratings = await _context.Reviews
                .Where(r => r.WasherId == washerId)
                .ToListAsync();
            if (ratings == null || ratings.Count == 0)
                return 0;
            return (double)ratings.Average(r => r.Rating);
        }
        public async Task<Review> AddCommentForReviewAsync(int reviewId, string comment)
        {
            // Retrieve the review by its ID.
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId);
            if (review == null)
                throw new Exception("Review not found.");
            // If a comment already exists, the admin should update it instead.
            if (!string.IsNullOrEmpty(review.Comment))
                throw new Exception("A comment already exists for this review. Use update instead.");
            // Add the new comment.
            review.Comment = comment;
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }
        public async Task<Review> UpdateCommentForReviewAsync(int reviewId, string newComment)
        {
            // Retrieve the review by its ID.
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.ReviewId== reviewId);
            if (review == null)
                throw new Exception("Review not found.");
            // Update the comment.
            review.Comment = newComment;
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }
        public async Task<Review> DeleteCommentForReviewAsync(int reviewId)
        {
            // Retrieve the review by its ID.
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId);
            if (review == null)
                throw new Exception("Review not found.");
            // "Delete" the comment by clearing it.
            review.Comment = string.Empty; // Alternatively, you could set it to null.
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }

        // Method to get WasherId by OrderId
        public async Task<int?> GetWasherIdByOrderIdAsync(int orderId)
        {
            // Find the order by OrderId and return the WasherId
            var order = await _context.Orders
                .Where(o => o.OrderId == orderId)
                .FirstOrDefaultAsync();

            return order?.WasherId;
        }

    }
}
