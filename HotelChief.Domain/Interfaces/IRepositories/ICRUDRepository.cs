namespace HotelChief.Core.Interfaces.IRepositories
{
    public interface ICRUDRepository<T>
    {
        Task<T?> GetByIdAsync(int id);

        Task<IEnumerable<T?>> GetAllAsync();

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);
    }
}
