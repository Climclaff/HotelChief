namespace HotelChief.Core.Entities
{
    public class ReviewGuestDownvote
    {
        public int? DownvoteId { get; set; }

        public int? ReviewId { get; set; }

        public int? GuestId { get; set; }
    }
}
