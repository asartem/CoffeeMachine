using System;
using System.Linq.Expressions;
using Domain.Common.Models;

namespace Domain.Common.Dal
{
    public interface IRepository<T> where T : IEntity
    {
        public IEntity Get(long id);
        public IEntity Get(Func<T,bool> predicate);
    
    }
}