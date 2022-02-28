namespace Cm.Api.Api.Products.Models
{
    /// <summary>
    /// Base class for Product DTOs
    /// </summary>
    public abstract  class ProductDtoBase
    {
        /// <summary>
        /// Name of the product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Price of the product. TODO: is it Cost or AmountAvailable?
        /// </summary>
        public decimal Price { get; set; }
        
        /// <summary>
        /// Quantity (Amount available?) TODO: is it Cost or AmountAvailable?
        /// </summary>
        public int Quantity { get; set; }

    }
}