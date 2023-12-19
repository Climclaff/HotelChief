namespace HotelChief.Infrastructure.EFEntities
{
    public class Review
    {
        public int ReviewId { get; set; }

        public Guest? Guest { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
