namespace HotelChief.Application.Services
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Core.Interfaces.IServices;

    public class RoomService : ICRUDService<Room>
    {
        private readonly ICRUDRepository<Room> repository;

        public RoomService(ICRUDRepository<Room> repository)
        {
            this.repository = repository;
        }

        public Task<Room?> GetByIdAsync(int entityId)
        {
            return repository.GetByIdAsync(entityId);
        }

        public Task<IEnumerable<Room?>> GetAllAsync()
        {
            return repository.GetAllAsync();
        }

        public Task AddAsync(Room entity)
        {
            return repository.AddAsync(entity);
        }

        public Task UpdateAsync(Room entity)
        {
            return repository.UpdateAsync(entity);
        }

        public Task DeleteAsync(int entityId)
        {
            return repository.DeleteAsync(entityId);
        }
    }
}
