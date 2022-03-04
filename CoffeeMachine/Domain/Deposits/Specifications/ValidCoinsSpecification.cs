using System.Linq;

namespace Cm.Domain.Deposits.Specifications
{
   
    /// <summary>
    /// Validates coins
    /// </summary>
    public class ValidCoinsSpecification : IValidCoinsSpecification
    {
        /// <summary>
        /// Returns check results
        /// </summary>
        /// <param name="coinValue"></param>
        /// <returns></returns>
        public bool IsSatisfiedBy(int coinValue)
        {
            var allowedCoins = new[] { 5, 10, 20, 50, 100 };
            if (coinValue < 1
                || allowedCoins.All(x => x != coinValue))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
