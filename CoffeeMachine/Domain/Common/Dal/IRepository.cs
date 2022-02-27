using System;
using System.Linq.Expressions;
using Domain.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Common.Dal
{
    public interface IRepository<T> where T : IEntity
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void Remove(T entity);


        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
    
    }
}