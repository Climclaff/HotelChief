namespace HotelChief.Core.Interfaces.IRepositories
{
    using HotelChief.Core.Entities.Identity;

    public interface IGuestRepository
    {
        Task<Guest?> GetByIdAsync(int id);

        Task<IEnumerable<Guest?>> GetAllAsync();

        Task UpdateAsync(Guest entity);

        Task DeleteAsync(int id);
    }
}
