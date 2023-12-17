namespace HotelChief.Application.Services
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Core.Interfaces.IServices;

    public class ReservationService : ICRUDService<Reservation>
    {
        private readonly ICRUDRepository<Reservation> repository;

        public ReservationService(ICRUDRepository<Reservation> repository)
        {
            this.repository = repository;
        }

        public Task<Reservation?> GetByIdAsync(int entityId)
        {
            return repository.GetByIdAsync(entityId);
        }

        public Task<IEnumerable<Reservation?>> GetAllAsync()
        {
            return repository.GetAllAsync();
        }

        public Task AddAsync(Reservation entity)
        {
            return repository.AddAsync(entity);
        }

        public Task UpdateAsync(Reservation entity)
        {
            return repository.UpdateAsync(entity);
        }

        public Task DeleteAsync(int entityId)
        {
            return repository.DeleteAsync(entityId);
        }
    }
}
