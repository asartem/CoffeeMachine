using System;
using System.ComponentModel.DataAnnotations;
using Cm.Domain.Products;
using static System.Int32;

namespace Cm.Api.Api.Products.Models
{
    /// <summary>
    /// updates product for coffee machine
    /// </summary>
    public class UpdateProductDto : ProductDtoBase
    {
        /// <summary>
        /// Name of the product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Price of the product
        /// </summary>
        [PriceValidation]
        public new int? Price { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        [Range(0, MaxValue, ErrorMessage = "Quantity should be more then zero")]
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

            existingProduct.Name = string.IsNullOrWhiteSpace(Name) ? existingProduct.Name : Name;
            existingProduct.Price = Price ?? existingProduct.Price;
            existingProduct.Qty = Quantity ?? existingProduct.Qty;
            return existingProduct;
        }
    }
}