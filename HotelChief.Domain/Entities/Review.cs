using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelChief.Domain.Entities
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
