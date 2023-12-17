namespace HotelChief.Infrastructure.Repositories
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class RoomRepository : ICRUDRepository<Room>
    {
        private readonly ApplicationDbContext context;

        public RoomRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            return await context.Rooms!.FindAsync(id) ?? null;
        }

        public async Task<IEnumerable<Room?>> GetAllAsync()
        {
            return await context.Rooms!.ToListAsync();
        }

        public async Task AddAsync(Room entity)
        {
            context.Rooms!.Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Room entity)
        {
            context.Rooms!.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var room = await context.Rooms!.FindAsync(id);
            if (room != null)
            {
                context.Rooms.Remove(room);
                await context.SaveChangesAsync();
            }
        }
    }
}
