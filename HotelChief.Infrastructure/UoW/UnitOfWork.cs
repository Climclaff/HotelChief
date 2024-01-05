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
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            RoomRepository = new RoomRepository(_context);
            ReservationRepository = new ReservationRepository(_context);
            ReviewRepository = new ReviewRepository(_context);
        }

        public IRoomRepository RoomRepository { get; private set; }

        public IReservationRepository ReservationRepository { get; private set; }

        public IReviewRepository ReviewRepository { get; private set; }

        public IBaseCRUDRepository<T> GetRepository<T>()
            where T : class
        {
            var repository = _serviceProvider.GetService(typeof(IBaseCRUDRepository<T>));
            if (repository == null)
            {
                repository = new BaseCrudRepository<T>(_context);
            }

            return (IBaseCRUDRepository<T>)repository;
        }

        public async void Dispose()
        {
            await _context.DisposeAsync();
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
