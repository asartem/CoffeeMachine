using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Common.Models;

namespace Domain.Common.Dal
{
    public abstract class Repository<T> : IRepository<T> where T : IEntity
    {
        public string ConnectionString { get; }

        protected Repository(IDbConnectionProvider dbConnectionProvider)
        {
            if (dbConnectionProvider == null)
            {
                throw new ArgumentNullException(nameof(dbConnectionProvider));
            }
            

            ConnectionString = dbConnectionProvider.ConnectionString;

        }
        public IEntity Get(long id)
        {
            IList<T> result = new List<T>();



            return result.SingleOrDefault();
        }


        public IEntity Get(Func<T,bool> predicate)
        {
            IList<T> result = new List<T>();



            return result.SingleOrDefault(predicate);
        }
    }
}