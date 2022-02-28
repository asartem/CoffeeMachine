namespace Api.Products.Models
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
    }
}