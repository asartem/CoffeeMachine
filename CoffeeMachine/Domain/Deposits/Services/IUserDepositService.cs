using Cm.Domain.Users;

namespace Cm.Domain.Deposits.Services
{
    /// <summary>
    /// Interface for deposit calculations
    /// </summary>
    public interface IUserDepositService
    {
        /// <summary>
        /// adds money to deposit
        /// </summary>
        /// <param name="user"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        User UpdateDeposit(User user, int amount);


        /// <summary>
        /// Reset deposit amount to 0
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        User ResetDeposit(User user);
    }
}