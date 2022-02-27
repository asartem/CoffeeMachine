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

        protected Product(){}
        public Product(string name, int userId)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));

            Name = name;
            UserId = userId;
        }

        //public Product(int id, string name) : this(name)
        //{
        //    if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

        //    Id = id;
        //}
    }
}
