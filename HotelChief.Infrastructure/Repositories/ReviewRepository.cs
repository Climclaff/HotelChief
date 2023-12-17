namespace HotelChief.Infrastructure.Repositories
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class ReviewRepository : ICRUDRepository<Review>
    {
        private readonly ApplicationDbContext context;

        public ReviewRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await context.Reviews!.FindAsync(id) ?? null;
        }

        public async Task<IEnumerable<Review?>> GetAllAsync()
        {
            return await context.Reviews!.ToListAsync();
        }

        public async Task AddAsync(Review entity)
        {
            context.Reviews!.Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Review entity)
        {
            context.Reviews!.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var review = await context.Reviews!.FindAsync(id);
            if (review != null)
            {
                context.Reviews.Remove(review);
                await context.SaveChangesAsync();
            }
        }
    }
}
