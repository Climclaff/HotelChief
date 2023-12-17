namespace HotelChief.Infrastructure.Repositories
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class EmployeeRepository : ICRUDRepository<Employee>
    {
        private readonly ApplicationDbContext context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await context.Employees!.FindAsync(id) ?? null;
        }

        public async Task<IEnumerable<Employee?>> GetAllAsync()
        {
            return await context.Employees!.ToListAsync();
        }

        public async Task AddAsync(Employee entity)
        {
            context.Employees!.Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee entity)
        {
            context.Employees!.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await context.Employees!.FindAsync(id);
            if (employee != null)
            {
                context.Employees.Remove(employee);
                await context.SaveChangesAsync();
            }
        }
    }
}
