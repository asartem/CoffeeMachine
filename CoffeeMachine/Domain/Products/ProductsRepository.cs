using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cm.Domain.Common.Dal;
using Microsoft.EntityFrameworkCore;

namespace Cm.Domain.Products
{
    /// <summary>
    /// Products repository
    /// </summary>
    public class ProductsRepository : IProductsRepository
    {
        /// <summary>
        /// Generic implementation of repository
        /// </summary>
        protected readonly IRepository<Product> GenericProductService;

        /// <summary>
        /// Context
        /// </summary>
        protected readonly ApplicationContext Context;

        /// <summary>
        /// Creates the instance of the class
        /// </summary>
        /// <param name="context"></param>
        /// <param name="genericProductService"></param>
        public ProductsRepository(ApplicationContext context, IRepository<Product> genericProductService)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            GenericProductService = genericProductService ?? throw new ArgumentNullException(nameof(genericProductService));
        }

        /// <summary>
        /// Return product by id for specific user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sellerId"></param>
        /// <returns></returns>
        public virtual async Task<Product> GetAsync(int id, int? sellerId = null)
        {
            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            Expression<Func<Product, bool>> predicate = x => x.Id == id;
            if (sellerId.HasValue)
            {
                predicate = x => x.Id == id && x.Seller.Id == sellerId.Value;
            }

            Product result = (await Context.Set<Product>()
                .Where(predicate)
                .Include(x => x.Seller)
                .SingleOrDefaultAsync());

            return result;
        }

        /// <summary>
        /// Return all products for all users
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<Product>> GetAllAsync()
        {

            IEnumerable<Product> result = (await Context.Set<Product>()
                .Include(x => x.Seller)
                .ToListAsync());

            return result;
        }
        
        /// <summary>
        /// Find product by expression for all sellers
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            
            var result = (await Context.Set<Product>()
                .Where(expression)
                .Include(x => x.Seller)
                .ToListAsync());

            return result;
        }

        /// <summary>
        /// Adds product
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task AddAsync(Product entity)
        {
            await GenericProductService.AddAsync(entity);
        }

        /// <summary>
        /// Adds product
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void AddToContext(Product entity)
        {
            GenericProductService.AddToContext(entity);
        }


        /// <summary>
        /// Removes product
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task RemoveAsync(Product entity)
        {
            await GenericProductService.RemoveAsync(entity);
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

        /// <summary>
        /// Cleanup repository
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }


}
