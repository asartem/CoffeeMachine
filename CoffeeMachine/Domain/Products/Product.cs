using System;
using Domain.Common.Models;
using Domain.Users;

namespace Domain.Products
{
    public class Product : IEntity
    {
        public int Id { get;  set; }
        public string Name { get;  set; }
        public User User { get; set; }
        public int UserId { get; set; }

        public Product(){}
        public Product(string name, int userId)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));

            Name = name;
            UserId = userId;
        }

        public Product(int id, string name, int userId):this(name,userId)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(userId));
            Id = id;
        }
    }
}
