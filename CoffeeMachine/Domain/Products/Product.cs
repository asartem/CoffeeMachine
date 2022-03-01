using System;
using Cm.Domain.Common.Models;

namespace Cm.Domain.Products
{
    /// <summary>
    /// Product entity
    /// </summary>
    public class Product : IEntity
    {
        /// <summary>
        /// id of the product
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the product
        /// </summary>
        public string Name { get; set; }
        

        /// <summary>
        /// Price (cost)
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// Products quantity
        /// </summary>
        public int Qty { get; set; }

        public Product() { }
        
        public Product(string name,  int price, int qty)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));;
            if (price < 0) throw new ArgumentOutOfRangeException(nameof(price));
            if (qty < 0) throw new ArgumentOutOfRangeException(nameof(qty));

            Name = name;
            Price = price;
            Qty = qty;
        }

        public Product(int id, string name,  int price, int qty) : this(name,  price, qty)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            Id = id;
        }
    }
}
