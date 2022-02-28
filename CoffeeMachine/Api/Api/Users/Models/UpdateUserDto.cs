using System;
using Cm.Domain.Users;

namespace Cm.Api.Api.Users.Models
{
    /// <summary>
    /// updates product for coffee machine
    /// </summary>
    public class UpdateUserDto : UserDtoBase
    {
        /// <summary>
        /// user password
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// Converts dto to entity
        /// </summary>
        /// <param name="existingUser"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public User Update(User existingUser)
        {
            if (existingUser == null)
            {
                throw new ArgumentNullException(nameof(existingUser));
            }
           
            existingUser.Password = string.IsNullOrWhiteSpace(Password) 
                                    ? existingUser.Name 
                                    : Password;
            existingUser.Deposit = Deposit ?? existingUser.Deposit;
            return existingUser;
        }
    }
}