#pragma warning disable SA1309
#pragma warning disable CS8618
namespace HotelChief.Infrastructure.UoW
{
    using HotelChief.Core.Interfaces;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Infrastructure.Data;
    using HotelChief.Infrastructure.Repositories;

    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private ApplicationDbContext _context;
        private Dictionary<Type, object> _repositories;
        private bool _disposed = false;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IBaseCRUDRepository<T> GetRepository<T>()
            where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (IBaseCRUDRepository<T>)_repositories[typeof(T)];
            }

            var repository = new BaseCrudRepository<T>(_context);
            _repositories.Add(typeof(T), repository);

            return repository;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        protected async virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    await _context.DisposeAsync();
                }
            }

            _disposed = true;
        }
    }
}
