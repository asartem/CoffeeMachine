using Cm.Domain.Common.Models;

namespace Cm.Domain.Users
{
    public class UserRole : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UserRole()
        {

        }
    }
}