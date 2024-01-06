namespace HotelChief.API.ViewModels.DTO
{
    using System.ComponentModel.DataAnnotations;

    public class ReviewDTO
    {
        public int ReviewId { get; set; }

        public int GuestId { get; set; }

        public string? GuestEmail { get; set; }

        [Required]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime Timestamp { get; set; }

        public int Upvotes { get; set; }

        public int Downvotes { get; set; }
    }
}
