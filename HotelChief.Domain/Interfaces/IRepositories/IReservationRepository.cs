using HotelChief.Core.Entities;

namespace HotelChief.Core.Interfaces.IRepositories
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Room>> GetAvailableRooms(DateTime checkInDate, DateTime checkOutDate);
    }
}
