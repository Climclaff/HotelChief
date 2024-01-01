using HotelChief.Core.Entities;
using HotelChief.Infrastructure.EFEntities;

namespace HotelChief.API.ViewModels
{
    public class ReviewViewModel
    {
        public int ReviewId { get; set; }

        public int GuestId { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime Timestamp { get; set; }

        public IEnumerable<Review>? Reviews { get; set; }
    }
}
