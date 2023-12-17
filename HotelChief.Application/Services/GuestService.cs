namespace HotelChief.Application.Services
{
    using HotelChief.Core.Entities.Identity;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Core.Interfaces.IServices;

    public class GuestService : IGuestService
    {
        private readonly IGuestRepository repository;

        public GuestService(IGuestRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Guest?> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id) ?? null;
        }

        public async Task<IEnumerable<Guest?>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task UpdateAsync(Guest entity)
        {
            await repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await repository.DeleteAsync(id);
        }
    }
}
