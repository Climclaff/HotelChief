namespace HotelChief.Infrastructure.EFEntities
{
    using HotelChief.Core.Entities;
    using Microsoft.AspNetCore.Identity;

    public class Guest : IdentityUser<int>
    {
        public string? FullName { get; set; }

        public double? LoyaltyPoints { get; set; }

        public ICollection<ReviewGuestUpvote> UpvotedReviews { get; set; } = new List<ReviewGuestUpvote>();

        public ICollection<ReviewGuestDownvote> DownvotedReviews { get; set; } = new List<ReviewGuestDownvote>();
    }
}
