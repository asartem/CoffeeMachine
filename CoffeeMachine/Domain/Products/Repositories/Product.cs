using System;
using Domain.Common.Models;

namespace Domain.Products.Repositories
{
    public class Product : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public Product(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        public Product(int id, string name) : this(name)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            Id = id;
        }
    }
}
