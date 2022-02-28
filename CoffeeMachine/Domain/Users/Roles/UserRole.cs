using Cm.Domain.Common.Models;

namespace Cm.Domain.Users.Roles
{
    /// <summary>
    /// User Role
    /// </summary>
    public class UserRole : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public UserRole() { }

        public UserRole(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    /// <summary>
    /// List of keys for UserRoles
    /// </summary>
    public static class UserRoles
    {
        public const string Buyer = "Buyer";
        public const string Seller = "Seller";
    }

}