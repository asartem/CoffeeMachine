using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cm.Domain.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Cm.Domain.Common.Dal
{
    public class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        public readonly ApplicationContext Context;

        public Repository(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        

        public virtual async Task<T> GetAsync(int id)
        {
            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            T result = await Context.Set<T>().FindAsync(id);
            return result;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var result = await Context.Set<T>().ToListAsync();
            return result;
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            IQueryable<T> queryable = Context.Set<T>().Where(expression);
            return await queryable.ToListAsync();
        }

        public virtual async Task AddAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            if (entity.Id == 0)
            {
                await Context.Set<T>().AddAsync(entity);
            }
            else
            {
                ContextManagementService.DetachLocal(Context, entity, entity.Id);
                Context.Set<T>().Update(entity);
            }

            await SaveContextAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (entity.Id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(entity.Id));
            }

            ContextManagementService.DetachLocal(Context, entity, entity.Id);
            Context.Set<T>().Remove(entity);
            await SaveContextAsync();
        }

        /// <summary>
        /// Adds to context without save
        /// </summary>
        /// <param name="entity"></param>
        public void AddToContext(T entity)
        {
            Context.Set<T>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Saves context
        /// </summary>
        /// <returns></returns>
        public async Task SaveContextAsync()
        {
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Flag for dispose
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Dispose context
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}