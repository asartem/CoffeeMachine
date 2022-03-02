using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cm.Domain.Products
{
    /// <summary>
    /// Interface for products repository
    /// </summary>
    public interface IProductsRepository
    {
        /// <summary>
        /// Returns products by id for user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
         Task<Product> GetAsync(int id, int userId);

         /// <summary>
         /// Return all products for all users
         /// </summary>
         /// <returns></returns>
         Task<IEnumerable<Product>> GetAllAsync();

         /// <summary>
         /// Find product by expression for specific user
         /// </summary>
         /// <param name="expression"></param>
         /// <param name="userId"></param>
         /// <returns></returns>
         Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> expression, int userId);

         /// <summary>
         /// Adds product
         /// </summary>
         /// <param name="entity"></param>
         /// <returns></returns>
         Task AddAsync(Product entity);

         /// <summary>
         /// Removes product
         /// </summary>
         /// <param name="entity"></param>
         /// <returns></returns>
         Task RemoveAsync(Product entity);
    }
}