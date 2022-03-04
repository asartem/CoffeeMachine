using Cm.Domain.Common.Models;

namespace Cm.Domain.Users.Roles
{
    /// <summary>
    /// User Role
    /// </summary>
    public class UserRole : IEntity
    {
        /// <summary>
        /// Role id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Role name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public UserRole() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public UserRole(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}