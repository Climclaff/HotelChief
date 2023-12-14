using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelChief.Domain.Entities.Abstract
{
    public abstract class Booking
    {
        public decimal Amount { get; set; }
        public bool PaymentStatus { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
