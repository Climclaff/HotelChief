namespace HotelChief.Infrastructure.EFEntities
{
    public class HotelServiceOrder
    {
        public int HotelServiceOrderId { get; set; }
        public Guest? Guest { get; set; }
        public HotelService? Service { get; set; }
        public Employee? Employee { get; set; }
        public DateTime ServiceOrderDate { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }

        public bool PaymentStatus { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
