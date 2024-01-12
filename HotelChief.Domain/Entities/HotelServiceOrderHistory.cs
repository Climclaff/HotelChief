namespace HotelChief.Core.Entities
{
    using HotelChief.Core.Entities.Abstract;

    public class HotelServiceOrderHistory : Booking
    {
        public int HotelServiceOrderHistoryId { get; set; }

        public int HotelServiceOrderId { get; set; }

        public int GuestId { get; set; }

        public string? OrderStatus { get; set; }

        public int HotelServiceId { get; set; }

        public int? EmployeeId { get; set; }

        public DateTime ServiceOrderDate { get; set; }

        public int Quantity { get; set; }
    }
}
