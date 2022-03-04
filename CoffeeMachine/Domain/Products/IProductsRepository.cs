﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cm.Domain.Products
{
    /// <summary>
    /// Interface for products repository
    /// </summary>
    public interface IProductsRepository : IDisposable
    {
        /// <summary>
        /// Returns products by id for user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sellerId"></param>
        /// <returns></returns>
        Task<Product> GetAsync(int id, int? sellerId = null);

        /// <summary>
        /// Return all products for all users
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetAllAsync();

        /// <summary>
        /// Find product by expression for specific user
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> expression);

        /// <summary>
        /// Adds product
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync(Product entity);

        /// <summary>
        /// Adds product to context without save
        /// </summary>
        /// <param name="entity"></param>
        void AddToContext(Product entity);

        /// <summary>
        /// Removes product
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task RemoveAsync(Product entity);
    }
}