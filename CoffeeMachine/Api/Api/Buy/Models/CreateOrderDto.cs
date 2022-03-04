using System.Collections.Generic;

namespace Cm.Api.Api.Buy.Models
{
    /// <summary>
    /// Model to create order
    /// </summary>
    public class CreateOrderDto
    {
        /// <summary>
        /// Product ids and quantity
        /// </summary>
        public IDictionary<int, int> ProductsAndQuantity { get; set; }

        /// <summary>
        /// Create the instance of the class
        /// </summary>
        public CreateOrderDto()
        {
            ProductsAndQuantity = new Dictionary<int, int>();
        }
    }
}
