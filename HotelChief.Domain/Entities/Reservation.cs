namespace HotelChief.Core.Entities
{
    using HotelChief.Core.Entities.Abstract;
    using HotelChief.Core.Entities.Identity;

    public class Reservation : Booking
    {
        public int ReservationId { get; set; }

        public Guest? Guest { get; set; }

        public Room? Room { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }
    }
}
