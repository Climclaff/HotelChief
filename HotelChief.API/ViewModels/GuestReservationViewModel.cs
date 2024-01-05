namespace HotelChief.API.ViewModels
{
    using HotelChief.Core.Entities;

    public class GuestReservationViewModel
    {
        public IEnumerable<Room>? AvailableRooms { get; set; }

        public Dictionary<int, IEnumerable<Tuple<DateTime, DateTime>>>? AvailableTimeSlots { get; set; }
    }
}
