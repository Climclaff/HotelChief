using HotelChief.Core.Entities;

namespace HotelChief.Core.Interfaces.IRepositories
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime checkInDate, DateTime checkOutDate);
    }
}
