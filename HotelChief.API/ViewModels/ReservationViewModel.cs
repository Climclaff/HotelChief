namespace HotelChief.API.ViewModels
{
    public class ReservationViewModel
    {
        public int ReservationId { get; set; }

        public int GuestId { get; set; }

        public int RoomNumber { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public decimal Amount { get; set; }

        public bool PaymentStatus { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
