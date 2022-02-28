using System;
using Cm.Domain.Users;
using Cm.Domain.Users.Roles;

namespace Cm.Api.Api.Users.Models
{
    /// <summary>
    /// Creates product for coffee machine
    /// </summary>
    public class CreateUserDto : UserDtoBase
    {
        /// <summary>
        /// Name of the user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Name of the user
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Name of the user
        /// </summary>
        public new int Deposit { get; set; }

        /// <summary>
        /// Converts dto to entity
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public User ToEntity(UserRole role)
        {
            User result = new User(UserName, Password, Deposit, role);
            return result;
        }

    }
}