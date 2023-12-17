namespace HotelChief.Infrastructure.Repositories
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class HotelServiceRepository : ICRUDRepository<HotelService>
    {
        private readonly ApplicationDbContext context;

        public HotelServiceRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<HotelService?> GetByIdAsync(int id)
        {
            return await context.HotelServices!.FindAsync(id) ?? null;
        }

        public async Task<IEnumerable<HotelService?>> GetAllAsync()
        {
            return await context.HotelServices!.ToListAsync();
        }

        public async Task AddAsync(HotelService entity)
        {
            context.HotelServices!.Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HotelService entity)
        {
            context.HotelServices!.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hotelService = await context.HotelServices!.FindAsync(id);
            if (hotelService != null)
            {
                context.HotelServices.Remove(hotelService);
                await context.SaveChangesAsync();
            }
        }
    }
}
