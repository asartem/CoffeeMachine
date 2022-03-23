using System;
using System.ComponentModel.DataAnnotations;
using Cm.Domain.Users;
using Cm.Domain.Users.Roles;

namespace Cm.Api.Api.Users.Models
{
    /// <summary>
    /// Creates user for coffee machine
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>
        /// Name of the user
        /// </summary>
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        /// <summary>
        /// Deposit of the user
        /// </summary>
        [Required(ErrorMessage = "Deposit is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Deposit should be more then 0")]
        public int Deposit { get; set; }

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