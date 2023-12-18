namespace HotelChief.Core.Interfaces
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Entities.Identity;
    using HotelChief.Core.Interfaces.IRepositories;

    public interface IUnitOfWork : IDisposable
    {
        Task Commit();

        ICRUDRepository<T> GetRepository<T>();
    }
}
