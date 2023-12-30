namespace HotelChief.Core.Entities
{
    using HotelChief.Core.Entities.Abstract;
    using HotelChief.Core.Entities.Identity;

    public class HotelServiceOrder : Booking
    {
        public int HotelServiceOrderId { get; set; }

        public int GuestId { get; set; }

        public HotelService? HotelService { get; set; }

        public int HotelServiceId { get; set; }

        public Employee? Employee { get; set; }

        public int EmployeeId { get; set; }

        public DateTime ServiceOrderDate { get; set; }

        public int Quantity { get; set; }
    }
}
