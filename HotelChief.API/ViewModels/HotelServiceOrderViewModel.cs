namespace HotelChief.API.ViewModels
{
    public class HotelServiceOrderViewModel
    {
        public int HotelServiceOrderId { get; set; }

        public int GuestId { get; set; }

        public int ServiceId { get; set; }

        public int EmployeeId { get; set; }

        public DateTime ServiceOrderDate { get; set; }

        public int Quantity { get; set; }

        public decimal Amount { get; set; }

        public bool PaymentStatus { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
