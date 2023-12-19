namespace HotelChief.Infrastructure.EFEntities
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        public Guest? Guest { get; set; }

        public Room? Room { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public decimal Amount { get; set; }

        public bool PaymentStatus { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
