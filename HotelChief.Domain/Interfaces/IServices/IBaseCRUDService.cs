namespace HotelChief.Core.Interfaces.IServices
{
    using System.Linq.Expressions;

    public interface IBaseCRUDService<T>
    {
        Task<IEnumerable<T>> Get(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "");

        Task AddAsync(T entity);

        void Update(T entity);

        Task DeleteAsync(int id);

        Task Commit();
    }
}
