using System;

namespace Cm.Domain.Purchases.Exceptions
{
    /// <summary>
    /// Exception when there is no enough funds
    /// </summary>
    public class InsufficientFoundsException : ApplicationException
    {
        /// <summary>
        /// Creates the new instance of the class
        /// </summary>
        /// <param name="message"></param>
        public InsufficientFoundsException(string s) : base(s)
        {
        }
    }
}