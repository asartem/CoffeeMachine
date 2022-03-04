using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cm.Domain.Common.Dal;
using Microsoft.EntityFrameworkCore;

namespace Cm.Domain.Users
{
    /// <summary>
    /// Users repository
    /// </summary>
    public class UsersRepository : Repository<User>, IUsersRepository
    {
        /// <summary>
        /// Create the instance of the class
        /// </summary>
        /// <param name="context"></param>
        public UsersRepository(ApplicationContext context) : base(context)
        {
        }

        /// <summary>
        /// Returns users with additional fields
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<User> GetAsync(int id)
        {
            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var q =
                Context.Set<User>()
                    .Where(x => x.Id == id)
                    .Include(x => x.Role);

            User result = await q.SingleOrDefaultAsync();
            return result;
        }
        

        /// <summary>
        /// Finds the required entities
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public override async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            IQueryable<User> queryable = Context.Set<User>()
                .Where(expression)
                .Include(x => x.Role);

            return await queryable.ToListAsync();
        }
    }
}
