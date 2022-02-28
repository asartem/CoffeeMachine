using System;
using Domain.Products;

namespace Api.Products.Models
{
    /// <summary>
    /// updates product for coffee machine
    /// </summary>
    public class UpdateProductDto : ProductDtoBase
    {
        /// <summary>
        /// Converts dto to entity
        /// </summary>
        /// <param name="existingProduct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public  Product Update(Product existingProduct)
        {
            if (existingProduct == null)
            {
                throw new ArgumentNullException(nameof(existingProduct));
            }

            existingProduct.Name = Name;
            return existingProduct;
        }
    }
}