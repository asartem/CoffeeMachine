using System;
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
        /// Seller, owner of products
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Price (cost)
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// Products quantity
        /// </summary>
        public int Qty { get; set; }

        public Product() { }
        
        public Product(string name, User user, int price, int qty)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (user.Id < 1) throw new ArgumentOutOfRangeException(nameof(user));
            if (price < 0) throw new ArgumentOutOfRangeException(nameof(price));
            if (qty < 0) throw new ArgumentOutOfRangeException(nameof(qty));

            Name = name;
            User = user;
            Price = price;
            Qty = qty;
        }

        public Product(int id, string name, User user, int price, int qty) : this(name, user, price, qty)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            Id = id;
        }
    }
}
