namespace HotelChief.Core.Interfaces
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Entities.Identity;
    using HotelChief.Core.Interfaces.IRepositories;

    public interface IUnitOfWork : IDisposable
    {
        IRoomRepository RoomRepository { get; }

        IReservationRepository ReservationRepository { get; }

        Task CommitAsync();

        IBaseCRUDRepository<T> GetRepository<T>()
            where T : class;
    }
}