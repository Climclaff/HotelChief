namespace HotelChief.Core.Entities
{
    public class ReviewGuestUpvote
    {
        public int UpvoteId { get; set; }

        public int? ReviewId { get; set; }

        public int? GuestId { get; set; }
    }
}
