using System;
using System.ComponentModel.DataAnnotations;
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
        /// Name of the product
        /// </summary>
        [Required(ErrorMessage = "Name of the product is required")]
        public new string Name { get; set; }

        /// <summary>
        /// Converts dto to entity
        /// </summary>
        /// <param name="seller"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Product ToEntity(User seller)
        {
            Product result = new Product(Name, seller, Price, Quantity);
            return result;
        }

    }
}