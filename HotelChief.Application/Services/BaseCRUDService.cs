#pragma warning disable IDE0052
#pragma warning disable SA1309 // Field names should not begin with underscore
namespace HotelChief.Application.Services
{
    using System.Linq.Expressions;
    using HotelChief.Core.Interfaces;
    using HotelChief.Core.Interfaces.IServices;

    public class BaseCRUDService<T> : ICRUDService<T>
    where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseCRUDService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(T entity)
        {
            await _unitOfWork.GetRepository<T>().AddAsync(entity);
            await _unitOfWork.Commit();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.GetRepository<T>().DeleteAsync(id);
            await _unitOfWork.Commit();
        }

        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "")
        {
            return await _unitOfWork.GetRepository<T>().Get(filter, orderBy, includeProperties);
        }

        public async Task UpdateAsync(T entity)
        {
            _unitOfWork.GetRepository<T>().Update(entity);
            await _unitOfWork.Commit();
        }
    }
}
