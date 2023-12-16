using HotelChief.Domain.Entities.Abstract;
using HotelChief.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelChief.Domain.Entities
{
    public class Reservation : Booking
    {
        public int ReservationId { get; set; }
        public Guest? Guest { get; set; }
        public Room? Room { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
