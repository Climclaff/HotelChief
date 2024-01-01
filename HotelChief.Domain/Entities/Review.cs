using HotelChief.Core.Entities.Identity;

namespace HotelChief.Core.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }

        public int GuestId { get; set; }

        public int Rating { get; set; }

        public int Upvotes { get; set; }

        public int Downvotes { get; set; }

        public string? Comment { get; set; }

        public DateTime Timestamp { get; set; }

        public ICollection<ReviewGuestUpvote>? Upvoters { get; set; }

        public ICollection<ReviewGuestDownvote>? Downvoters { get; set; }
    }
}
