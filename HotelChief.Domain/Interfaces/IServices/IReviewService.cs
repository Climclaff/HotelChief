namespace HotelChief.Core.Interfaces.IServices
{
    using HotelChief.Core.Entities;

    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetReviewsAsync();

        Task AddReviewAsync(Review review);

        Task DeleteReviewAsync(Review review);

        Task CommitAsync();

        Task<Review> GetReviewByIdAsync(int id);

        Task<int> UpvoteReviewAsync(int reviewId, int userId);

        Task<int> DownvoteReviewAsync(int reviewId, int userId);
    }
}
