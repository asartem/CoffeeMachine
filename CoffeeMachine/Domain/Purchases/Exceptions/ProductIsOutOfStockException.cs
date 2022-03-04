using System;

namespace Cm.Domain.Purchases.Exceptions
{
    /// <summary>
    /// Exception in case when there is no 
    /// </summary>
    public class ProductIsOutOfStockException : ApplicationException
    {
        /// <summary>
        /// Creates the new instance of the class
        /// </summary>
        /// <param name="message"></param>
        public ProductIsOutOfStockException(string message) : base(message)
        {
        }
    }
}