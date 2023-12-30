namespace HotelChief.Infrastructure.Repositories
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
