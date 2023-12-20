using HotelChief.Infrastructure.EFEntities;

namespace HotelChief.API.ViewModels
{
    public class ReviewViewModel
    {
        public int ReviewId { get; set; }

        public Guest? Guest { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
