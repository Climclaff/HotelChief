namespace HotelChief.Infrastructure.Repositories
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Entities.Identity;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class GuestRepository : IGuestRepository
    {
        private readonly ApplicationDbContext _context;

        public GuestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task RemoveGuestReviewVotesAsync(int guestId)
        {
            var downvotesToRemove = await _context.ReviewDownvotes.Where(x => x.GuestId == guestId).ToListAsync();
            var upvotesToRemove = await _context.ReviewUpvotes.Where(x => x.GuestId == guestId).ToListAsync();

            var reviewIdsToChangeCounter = downvotesToRemove.Select(vote => vote.ReviewId)
                                                 .Concat(upvotesToRemove.Select(vote => vote.ReviewId))
                                                 .Distinct();

            var reviewsToChangeCounter = await _context.Reviews
                .Where(review => reviewIdsToChangeCounter.Contains(review.ReviewId))
                .ToListAsync();

            foreach (var vote in downvotesToRemove)
            {
                var correspondingReview = reviewsToChangeCounter
                    .FirstOrDefault(review => review.ReviewId == vote.ReviewId);

                if (correspondingReview != null)
                {
                    correspondingReview.Downvotes -= 1;
                }
            }

            foreach (var vote in upvotesToRemove)
            {
                var correspondingReview = reviewsToChangeCounter
                    .FirstOrDefault(review => review.ReviewId == vote.ReviewId);

                if (correspondingReview != null)
                {
                    correspondingReview.Upvotes -= 1;
                }
            }

            _context.UpdateRange(reviewsToChangeCounter);
            _context.RemoveRange(downvotesToRemove);
            _context.RemoveRange(upvotesToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveEmployeeInfoAsync(int guestId)
        {
            var employee = await _context.Employees.Where(x => x.GuestId == guestId).FirstOrDefaultAsync();
            if (employee != null)
            {
                _context.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }
    }
}
