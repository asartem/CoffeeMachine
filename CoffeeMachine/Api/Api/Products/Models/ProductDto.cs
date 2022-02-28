using System;
using System.Collections.Generic;
using Cm.Domain.Products;
using Newtonsoft.Json;

namespace Cm.Api.Api.Products.Models
{
    /// <summary>
    /// Product for coffee machine
    /// </summary>
    public class ProductDto : ProductDtoBase
    {
        /// <summary>
        /// Id of the product
        /// </summary>
        public int Id { get; set; }
        

        /// <summary>
        /// Creates instance of the dto
        /// </summary>
        [JsonConstructor]
        protected ProductDto() { }

        /// <summary>
        /// Converts entity to DTO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ProductDto(Product entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Id = entity.Id;
            Name = entity.Name;
            Price = entity.Price;
            Quantity = entity.Qty;
        }
        

        /// <summary>
        /// Convert entities collection to dto
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public static IEnumerable<ProductDto> ToDto(IEnumerable<Product> products)
        {
            foreach (Product product in products)
            {
                var dto = new ProductDto(product);
                yield return dto;
            }
        }
    }
}
