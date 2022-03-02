using System;

namespace Cm.Domain.Purchases.Exceptions
{
    /// <summary>
    /// Exception when there is no enough funds
    /// </summary>
    public class InsufficientFoundsException : ApplicationException
    {
        public InsufficientFoundsException(string s) : base(s)
        {
        }
    }
}