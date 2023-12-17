namespace HotelChief.Infrastructure.Repositories
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class HotelServiceOrderRepository : ICRUDRepository<HotelServiceOrder>
    {
        private readonly ApplicationDbContext context;

        public HotelServiceOrderRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<HotelServiceOrder?> GetByIdAsync(int id)
        {
            return await context.HotelServiceOrders!.FindAsync(id) ?? null;
        }

        public async Task<IEnumerable<HotelServiceOrder?>> GetAllAsync()
        {
            return await context.HotelServiceOrders!.ToListAsync();
        }

        public async Task AddAsync(HotelServiceOrder entity)
        {
            context.HotelServiceOrders!.Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HotelServiceOrder entity)
        {
            context.HotelServiceOrders!.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hotelServiceOrder = await context.HotelServiceOrders!.FindAsync(id);
            if (hotelServiceOrder != null)
            {
                context.HotelServiceOrders.Remove(hotelServiceOrder);
                await context.SaveChangesAsync();
            }
        }
    }
}
