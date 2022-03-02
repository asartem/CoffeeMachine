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
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task<Product> GetAsync(int id, int userId)
        {
            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            Product result = (await GenericProductService.FindAsync(x => x.Id == id
                                                                         && x.Seller.Id == userId))
                                    .SingleOrDefault();
            
            return result;
        }

        /// <summary>
        /// Return all products for all users
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<Product>> GetAllAsync()
        {
            List<Product> result = 
                (await GenericProductService.GetAllAsync())
                .ToList();

            return result;
        }

        /// <summary>
        /// Find product by expression for specific user
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> expression, int userId)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            
            IQueryable<Product> queryable = Context.Set<Product>()
                .Where(expression)
                .Where(x => x.Seller.Id == userId);

            return await queryable.ToListAsync();
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

            IQueryable<Product> queryable = Context.Set<Product>()
                .Where(expression);

            return await queryable.ToListAsync();
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
    }


}
