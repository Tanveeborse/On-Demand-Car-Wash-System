using GreenWashAPI.Models;

namespace GreenWashAPI.Interface
{
    public interface IReviews
    {
        Task<Review> CreateReviewAsync(Review review);
        Task<Review> GetReviewByIdAsync(int reviewId);
        Task<double> GetAverageRatingForWasherAsync(int washerId);
        Task<Review> AddCommentForReviewAsync(int reviewId, string comment);
        Task<Review> UpdateCommentForReviewAsync(int reviewId, string newComment);
        Task<Review> DeleteCommentForReviewAsync(int reviewId);
        Task<int?> GetWasherIdByOrderIdAsync(int orderId);
    }
}
