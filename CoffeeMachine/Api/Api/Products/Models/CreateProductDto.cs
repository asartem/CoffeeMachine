using System;
using Cm.Domain.Products;

namespace Cm.Api.Api.Products.Models
{
    /// <summary>
    /// Creates product for coffee machine
    /// </summary>
    public class CreateProductDto : ProductDtoBase
    {
        /// <summary>
        /// Converts dto to entity
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Product ToEntity(int userId)
        {
            Product result = new Product(Name, userId, Price, Quantity);
            return result;
        }

    }
}