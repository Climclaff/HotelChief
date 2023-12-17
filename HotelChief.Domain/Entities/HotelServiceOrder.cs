using HotelChief.Core.Entities.Abstract;
using HotelChief.Core.Entities.Identity;

namespace HotelChief.Core.Entities
{
    public class HotelServiceOrder : Booking
    {
        public int HotelServiceOrderId { get; set; }
        public Guest? Guest { get; set; }
        public HotelService? Service { get; set; }
        public Employee? Employee { get; set; }
        public DateTime ServiceOrderDate { get; set; }
        public int Quantity { get; set; }
    }
}
