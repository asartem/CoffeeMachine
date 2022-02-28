using System;
using Cm.Domain.Common.Models;
using Cm.Domain.Users;

namespace Cm.Domain.Products
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }

        public Product() { }
        public Product(string name, int userId, decimal price, int qty)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));
            if (price < 0) throw new ArgumentOutOfRangeException(nameof(userId));
            if (qty < 0) throw new ArgumentOutOfRangeException(nameof(userId));

            Name = name;
            UserId = userId;
            Price = price;
            Qty = qty;
        }

        public Product(int id, string name, int userId, decimal price, int qty) : this(name, userId, price, qty)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(userId));
            Id = id;
        }
    }
}
