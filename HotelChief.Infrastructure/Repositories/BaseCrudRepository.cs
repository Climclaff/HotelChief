#pragma warning disable SA1309 // Field names should not begin with underscore
namespace HotelChief.Infrastructure.Repositories
{
    using System.Linq.Expressions;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class BaseCrudRepository<T> : IBaseCRUDRepository<T>
        where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> dbSet;

        public BaseCrudRepository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
        }

        public async virtual Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async virtual Task DeleteAsync(int id)
        {
            T? entityToDelete = await dbSet.FindAsync(id);
            if (entityToDelete != null)
            {
                dbSet.Remove(entityToDelete);
            }
        }

        public async virtual Task<IReadOnlyList<T>> Get(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(
                new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async virtual Task<T?> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual void DeleteAll()
        {
            foreach (var entity in dbSet)
            {
                dbSet.Remove(entity);
            }
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
