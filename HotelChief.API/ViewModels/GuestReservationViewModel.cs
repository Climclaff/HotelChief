namespace HotelChief.API.ViewModels
{
    using HotelChief.Core.Entities;
    using System.Collections.Concurrent;

    public class GuestReservationViewModel
    {
        public IEnumerable<Room>? AvailableRooms { get; set; }

        public ConcurrentDictionary<int, IEnumerable<Tuple<DateTime, DateTime>>>? AvailableTimeSlots { get; set; }
    }
}
