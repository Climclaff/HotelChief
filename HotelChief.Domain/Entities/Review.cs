namespace HotelChief.Core.Entities
{
    using HotelChief.Core.Entities.Identity;

    public class Review
    {
        public int ReviewId { get; set; }

        public int GuestId { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
