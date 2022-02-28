using System;
using System.Linq;
using System.Threading.Tasks;
using Cm.Domain.Common.Dal;
using Cm.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Cm.Domain.Products
{
    /// <summary>
    /// Products repository
    /// </summary>
    public class ProductsRepository : Repository<Product>, IProductsRepository
    {
        /// <summary>
        /// Creates the instance of the class
        /// </summary>
        /// <param name="context"></param>
        public ProductsRepository(ApplicationContext context) : base(context)
        {
        }

        /// <summary>
        /// Get product by id for appropriate user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<Product> GetAsync(int id)
        {
            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            IIncludableQueryable<Product, User> q =
                Context.Set<Product>()
                    .Where(x => x.Id == id)
                    .Include(x => x.User);

            Product result = await q.SingleOrDefaultAsync();
            return result;
        }
    }


}
