namespace HotelChief.Core.Interfaces.IServices
{
    public interface ICRUDService<T>
    {
        Task<T?> GetByIdAsync(int id);

        Task<IEnumerable<T?>> GetAllAsync();

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);
    }
}
