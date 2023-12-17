namespace HotelChief.Application.Services
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Core.Interfaces.IServices;

    public class ReviewService : ICRUDService<Review>
    {
        private readonly ICRUDRepository<Review> repository;

        public ReviewService(ICRUDRepository<Review> repository)
        {
            this.repository = repository;
        }

        public Task<Review?> GetByIdAsync(int entityId)
        {
            return repository.GetByIdAsync(entityId);
        }

        public Task<IEnumerable<Review?>> GetAllAsync()
        {
            return repository.GetAllAsync();
        }

        public Task AddAsync(Review entity)
        {
            return repository.AddAsync(entity);
        }

        public Task UpdateAsync(Review entity)
        {
            return repository.UpdateAsync(entity);
        }

        public Task DeleteAsync(int entityId)
        {
            return repository.DeleteAsync(entityId);
        }
    }
}
