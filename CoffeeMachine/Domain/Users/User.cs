using System;
using System.Threading;
using Cm.Domain.Common.Models;
using Cm.Domain.Purchases;
using Cm.Domain.Purchases.Exceptions;
using Cm.Domain.Users.Roles;

namespace Cm.Domain.Users
{
    /// <summary>
    /// User (buyer or seller)
    /// </summary>
    public class User : IEntity
    {
        /// <summary>
        /// Id of the user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User name (login)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// User's deposit
        /// </summary>
        public int Deposit { get; set; }
        
        /// <summary>
        /// Role 
        /// </summary>
        public UserRole Role { get; set; } // TODO: can be extended to 1-to-many Roles per User in future

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

        /// <summary>
        /// Reduces deposit
        /// </summary>
        /// <param name="amount"></param>
        public void ReduceDeposit(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            if (Deposit < amount)
            {
                throw new InsufficientFoundsException("Available deposit amount is less than amount you try to withdraw");
            }
            Deposit -= amount;
        }
    }
}
