using System;
using Cm.Domain.Products;
using Cm.Domain.Users;

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
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Product ToEntity(User user)
        {
            Product result = new Product(Name, user, Price, Quantity);
            return result;
        }

    }
}