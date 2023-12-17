namespace HotelChief.Core.Entities.Abstract
{
    public abstract class Booking
    {
        public decimal Amount { get; set; }

        public bool PaymentStatus { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
