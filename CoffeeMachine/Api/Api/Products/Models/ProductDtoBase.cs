using System.ComponentModel.DataAnnotations;
using static System.Int32;

namespace Cm.Api.Api.Products.Models
{
    /// <summary>
    /// Base class for Product DTOs
    /// </summary>
    public abstract class ProductDtoBase
    {
        /// <summary>
        /// Name of the product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Price of the product (Cost). TODO:  ask is it Cost or AmountAvailable?
        /// </summary>
        [PriceValidation]
        public int Price { get; set; }

        /// <summary>
        /// Quantity (Amount available?) TODO: ask  is it Cost or AmountAvailable?
        /// </summary>
        [Range(0, MaxValue, ErrorMessage = "Quantity should be more then zero")]
        public int Quantity { get; set; }

    }
}