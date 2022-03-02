using System;
using System.Threading;
using Cm.Domain.Common.Models;
using Cm.Domain.Users;

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

        /// <summary>
        /// Owner of the product
        /// </summary>
        public User Seller { get; set; }

        public Product() { }

        public Product(string name, User seller, int price, int qty)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name)); ;
            if (price < 0) throw new ArgumentOutOfRangeException(nameof(price));
            if (qty < 0) throw new ArgumentOutOfRangeException(nameof(qty));

            Name = name;
            Price = price;
            Qty = qty;
            Seller = seller ?? throw new ArgumentNullException(nameof(qty));
        }

        public Product(int id, string name, User seller, int price, int qty) : this(name, seller, price, qty)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            Id = id;
        }

        /// <summary>
        /// Reduce qty for product
        /// </summary>
        /// <param name="qty"></param>
        public void ReduceQty(int qty)
        {
            if (qty < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(qty));
            }
            if (Qty < qty)
            {
                throw new ArgumentOutOfRangeException("Product qty is less then purchased qty");
            }

            Qty -= qty;
        }
    }
}
