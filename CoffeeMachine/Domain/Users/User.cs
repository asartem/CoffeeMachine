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
        
        //public ICollection<Product> Products { get; protected set;}


        public User()
        {

        }
    }
}
