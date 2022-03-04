using System;
using Cm.Domain.Deposits.Specifications;
using Cm.Domain.Users;

namespace Cm.Domain.Deposits.Services
{
    /// <summary>
    /// Interface for deposit calculations
    /// </summary>
    public class UserDepositService : IUserDepositService
    {
        /// <summary>
        /// Coins specification
        /// </summary>
        public IValidCoinsSpecification Specification { get; }

        /// <summary>
        /// Checks specification
        /// </summary>
        /// <param name="specification"></param>
        public UserDepositService(IValidCoinsSpecification specification)
        {
            Specification = specification ?? throw new ArgumentNullException(nameof(specification));
        }

        /// <summary>
        /// adds money to deposit
        /// </summary>
        /// <param name="user"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public User UpdateDeposit(User user, int amount)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (Specification.IsSatisfiedBy(amount) == false)
            {
                throw new ArgumentOutOfRangeException("Only coins 5,10,20,50 and 100 are allowed");
            }

            user.Deposit += amount;
            return user;
        }

        /// <summary>
        /// Reset deposit amount to 0
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User ResetDeposit(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Deposit = 0;
            return user;
        }
    }
}
