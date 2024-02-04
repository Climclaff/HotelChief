namespace HotelChief.Application.Services
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Core.Interfaces.IServices;

    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CommitAsync()
        {
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsAsync()
        {
            var reviews = await _unitOfWork.GetRepository<Review>().GetAsync();
            return reviews;
        }

        public async Task AddReviewAsync(Review review)
        {
            await _unitOfWork.GetRepository<Review>().AddAsync(review);
        }

        public async Task DeleteReviewAsync(Review review)
        {
            await _unitOfWork.GetRepository<Review>().DeleteAsync(review.ReviewId);
        }

        public async Task<Review> GetReviewByIdAsync(int id)
        {
            return (await _unitOfWork.GetRepository<Review>().GetAsync(
                   r => r.ReviewId == id,
                   includeProperties: ""
               )).Select(r => new Review
               {
                   ReviewId = r.ReviewId,
                   Rating = r.Rating,
                   Downvotes = r.Downvotes,
                   Upvotes = r.Upvotes,
                   Comment = r.Comment,
                   GuestId = r.GuestId,
                   Timestamp = r.Timestamp,
                   Upvoters = r.Upvoters,
                   Downvoters = r.Downvoters
               }).FirstOrDefault();
        }

        public async Task<int> UpvoteReviewAsync(int reviewId, int userId)
        {
            var review = (await _unitOfWork.GetRepository<Review>().GetAsync(r => r.ReviewId == reviewId)).FirstOrDefault();
            if (review == null)
            {
                throw new InvalidOperationException("Review not found.");
            }
            ReviewGuestDownvote downvoter = (await _unitOfWork.GetRepository<ReviewGuestDownvote>().GetAsync(r => r.ReviewId == reviewId && r.GuestId == userId)).FirstOrDefault();
            if (downvoter != null)
            {
                await _unitOfWork.GetRepository<ReviewGuestDownvote>().DeleteAsync((int)downvoter.DownvoteId);
                review.Downvotes--;
            }

            ReviewGuestUpvote upvoter = (await _unitOfWork.GetRepository<ReviewGuestUpvote>().GetAsync(r => r.ReviewId == reviewId && r.GuestId == userId)).FirstOrDefault();
            if (upvoter != null)
            {
                return review.Upvotes;
            }

            review.Upvotes++;
            ReviewGuestUpvote upvote = new ReviewGuestUpvote() { ReviewId = reviewId, GuestId = userId };
            await _unitOfWork.GetRepository<ReviewGuestUpvote>().AddAsync(upvote);
            await _unitOfWork.CommitAsync();

            return review.Upvotes;
        }

        public async Task<int> DownvoteReviewAsync(int reviewId, int userId)
        {
            var review = (await _unitOfWork.GetRepository<Review>().GetAsync(r => r.ReviewId == reviewId)).FirstOrDefault();

            if (review == null)
            {
                throw new InvalidOperationException("Review not found.");
            }

            ReviewGuestUpvote upvoter = (await _unitOfWork.GetRepository<ReviewGuestUpvote>().GetAsync(r => r.ReviewId == reviewId && r.GuestId == userId)).FirstOrDefault();
            if (upvoter != null)
            {
                await _unitOfWork.GetRepository<ReviewGuestUpvote>().DeleteAsync((int)upvoter.UpvoteId);
                review.Upvotes--;
            }

            ReviewGuestDownvote downvoter = (await _unitOfWork.GetRepository<ReviewGuestDownvote>().GetAsync(r => r.ReviewId == reviewId && r.GuestId == userId)).FirstOrDefault();
            if (downvoter != null)
            {
                return review.Downvotes;
            }

            review.Downvotes++;
            ReviewGuestDownvote downvote = new ReviewGuestDownvote() { ReviewId = reviewId, GuestId = userId };
            await _unitOfWork.GetRepository<ReviewGuestDownvote>().AddAsync(downvote);
            await _unitOfWork.CommitAsync();

            return review.Downvotes;
        }
    }
}
