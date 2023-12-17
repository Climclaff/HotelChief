namespace HotelChief.Infrastructure.Repositories
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class ReservationRepository : ICRUDRepository<Reservation>
    {
        private readonly ApplicationDbContext context;

        public ReservationRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await context.Reservations!.FindAsync(id) ?? null;
        }

        public async Task<IEnumerable<Reservation?>> GetAllAsync()
        {
            return await context.Reservations!.ToListAsync();
        }

        public async Task AddAsync(Reservation entity)
        {
            context.Reservations!.Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reservation entity)
        {
            context.Reservations!.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var reservation = await context.Reservations!.FindAsync(id);
            if (reservation != null)
            {
                context.Reservations.Remove(reservation);
                await context.SaveChangesAsync();
            }
        }
    }
}
