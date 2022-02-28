using System;
using Domain.Common.Models;

namespace Domain.Users
{
    public class User : IEntity
    {
        public int Id { get;  set; }
        public string Name { get;  set;}
        public string Password { get; set; }
        public string Email { get;  set;}
        public UserRole Role { get;  set;}

        public User()
        {

        }

        public User(string name, string password, string email, UserRole role)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Role = role ?? throw new ArgumentNullException(nameof(role));
        }
    }
}
