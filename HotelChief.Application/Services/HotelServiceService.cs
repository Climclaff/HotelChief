namespace HotelChief.Application.Services
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Core.Interfaces.IServices;

    public class HotelServiceService : ICRUDService<HotelService>
    {
        private readonly ICRUDRepository<HotelService> repository;

        public HotelServiceService(ICRUDRepository<HotelService> repository)
        {
            this.repository = repository;
        }

        public Task<HotelService?> GetByIdAsync(int entityId)
        {
            return repository.GetByIdAsync(entityId);
        }

        public Task<IEnumerable<HotelService?>> GetAllAsync()
        {
            return repository.GetAllAsync();
        }

        public Task AddAsync(HotelService entity)
        {
            return repository.AddAsync(entity);
        }

        public Task UpdateAsync(HotelService entity)
        {
            return repository.UpdateAsync(entity);
        }

        public Task DeleteAsync(int entityId)
        {
            return repository.DeleteAsync(entityId);
        }
    }
}
