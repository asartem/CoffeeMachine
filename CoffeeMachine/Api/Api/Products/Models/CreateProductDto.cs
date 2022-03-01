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
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Product ToEntity()
        {
            Product result = new Product(Name, Price, Quantity);
            return result;
        }

    }
}