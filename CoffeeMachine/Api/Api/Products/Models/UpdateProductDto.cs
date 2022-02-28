using System;
using Cm.Domain.Products;

namespace Cm.Api.Api.Products.Models
{
    /// <summary>
    /// updates product for coffee machine
    /// </summary>
    public class UpdateProductDto : ProductDtoBase
    {
        /// <summary>
        /// Price of the product
        /// </summary>
        public new int? Price { get; set; }
        
        /// <summary>
        /// Quantity
        /// </summary>
        public new int? Quantity { get; set; }

        /// <summary>
        /// Converts dto to entity
        /// </summary>
        /// <param name="existingProduct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Product Update(Product existingProduct)
        {
            if (existingProduct == null)
            {
                throw new ArgumentNullException(nameof(existingProduct));
            }

            existingProduct.Name = Name ?? existingProduct.Name;
            existingProduct.Price = Price ?? existingProduct.Price;
            existingProduct.Qty = Quantity ?? existingProduct.Qty;
            return existingProduct;
        }
    }
}