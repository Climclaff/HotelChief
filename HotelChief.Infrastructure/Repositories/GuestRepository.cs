namespace HotelChief.Infrastructure.Repositories
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Entities.Identity;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class GuestRepository : IGuestRepository
    {
        private readonly ApplicationDbContext context;

        public GuestRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Guest?> GetByIdAsync(int id)
        {
            return await context.Users!.FindAsync(id) ?? null;
        }

        public async Task<IEnumerable<Guest?>> GetAllAsync()
        {
            return await context.Users!.ToListAsync();
        }

        public async Task UpdateAsync(Guest entity)
        {
            context.Users!.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await context.Users!.FindAsync(id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }
    }
}
