namespace Cm.Domain.Deposits.Specifications
{
    /// <summary>
    /// Validates coins
    /// </summary>
    public interface IValidCoinsSpecification
    {
        /// <summary>
        /// Returns check results
        /// </summary>
        /// <param name="coinValue"></param>
        /// <returns></returns>
        bool IsSatisfiedBy(int coinValue);
    }
}