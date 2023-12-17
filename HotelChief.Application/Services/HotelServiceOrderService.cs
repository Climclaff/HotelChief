namespace HotelChief.Application.Services
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Core.Interfaces.IServices;

    public class HotelServiceOrderService : ICRUDService<HotelServiceOrder>
    {
        private readonly ICRUDRepository<HotelServiceOrder> repository;

        public HotelServiceOrderService(ICRUDRepository<HotelServiceOrder> repository)
        {
            this.repository = repository;
        }

        public Task<HotelServiceOrder?> GetByIdAsync(int entityId)
        {
            return repository.GetByIdAsync(entityId);
        }

        public Task<IEnumerable<HotelServiceOrder?>> GetAllAsync()
        {
            return repository.GetAllAsync();
        }

        public Task AddAsync(HotelServiceOrder entity)
        {
            return repository.AddAsync(entity);
        }

        public Task UpdateAsync(HotelServiceOrder entity)
        {
            return repository.UpdateAsync(entity);
        }

        public Task DeleteAsync(int entityId)
        {
            return repository.DeleteAsync(entityId);
        }
    }
}
