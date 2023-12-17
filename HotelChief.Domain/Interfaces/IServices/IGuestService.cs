namespace HotelChief.Core.Interfaces.IServices
{
    using HotelChief.Core.Entities.Identity;

    public interface IGuestService
    {
        Task<Guest?> GetByIdAsync(int id);

        Task<IEnumerable<Guest?>> GetAllAsync();

        Task UpdateAsync(Guest entity);

        Task DeleteAsync(int id);
    }
}
