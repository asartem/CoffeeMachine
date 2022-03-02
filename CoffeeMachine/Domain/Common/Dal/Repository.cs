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

        #region SyncOperations
        public virtual T Get(int id)
        {
            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            T result = Context.Set<T>().Find(id);
            return result;
        }


        public virtual IEnumerable<T> GetAll()
        {
            var result = Context.Set<T>().ToList();
            return result;
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            IQueryable<T> result = Context.Set<T>().Where(expression);
            return result;
        }

        public virtual void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Set<T>().Add(entity);
        }

        public virtual void Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity.Id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(entity.Id));
            }

            Context.Set<T>().Remove(entity);
            Context.SaveChanges();
        }
        #endregion

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
            await Context.SaveChangesAsync();
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
            await Context.SaveChangesAsync();
        }


        protected void DetachLocal(T t, int entryId)
        {
            var local = Context.Set<T>()
                .Local
                .FirstOrDefault(entry => entry.Id == entryId);

            if (local != null)
            {
                Context.Entry(local).State = EntityState.Detached;
            }
            Context.Entry(t).State = EntityState.Modified;
        }
    }
}