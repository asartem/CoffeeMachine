using System;
using Cm.Domain.Common.Models;
using Cm.Domain.Users.Roles;

namespace Cm.Domain.Users
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Deposit { get; set; }

        public int RoleId { get; set; }
        public virtual UserRole Role { get; set; } // TODO: can be extended to 1-to-many Roles per User in future

        public User()
        {

        }

        public User(string name, string password, int deposit, UserRole role)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            Deposit = deposit < 0 ? throw new ArgumentOutOfRangeException(nameof(deposit)) : deposit;
            Role = role ?? throw new ArgumentNullException(nameof(role));
        }
    }
}
