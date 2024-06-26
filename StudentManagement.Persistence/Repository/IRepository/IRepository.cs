﻿//Developed by Md. Ashik
using System.Linq.Expressions;

namespace StudentManagement.Persistence.Repository.IRepository
{
    public interface IRepository<T> where T: class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null , int pageSize = 3, int pageNumber = 1);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task CreateAsync(T entity);

        

        Task RemoveAsync(T entity);

        Task SaveAsync();
    }
}
