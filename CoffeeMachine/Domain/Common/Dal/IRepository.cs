using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cm.Domain.Common.Models;

namespace Cm.Domain.Common.Dal
{
    /// <summary>
    /// Generic repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> : IDisposable where T : IEntity
    {
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task RemoveAsync(T entity);
        void AddToContext(T entity);
        Task SaveContextAsync();

    }
}