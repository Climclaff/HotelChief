﻿namespace HotelChief.Core.Interfaces.IRepositories
{
    using System.Linq.Expressions;

    public interface IBaseCRUDRepository<T>
    {
        Task<IReadOnlyList<T>> GetAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "");

        Task AddAsync(T entity);

        void Update(T entity);

        Task DeleteAsync(int id);

        void DeleteAll();
    }
}
