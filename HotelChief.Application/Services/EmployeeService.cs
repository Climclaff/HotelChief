namespace HotelChief.Application.Services
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Core.Interfaces.IServices;

    public class EmployeeService : ICRUDService<Employee>
    {
        private readonly ICRUDRepository<Employee> repository;

        public EmployeeService(ICRUDRepository<Employee> repository)
        {
            this.repository = repository;
        }

        public Task<Employee?> GetByIdAsync(int entityId)
        {
            return repository.GetByIdAsync(entityId);
        }

        public Task<IEnumerable<Employee?>> GetAllAsync()
        {
            return repository.GetAllAsync();
        }

        public Task AddAsync(Employee entity)
        {
            return repository.AddAsync(entity);
        }

        public Task UpdateAsync(Employee entity)
        {
            return repository.UpdateAsync(entity);
        }

        public Task DeleteAsync(int entityId)
        {
            return repository.DeleteAsync(entityId);
        }
    }
}
