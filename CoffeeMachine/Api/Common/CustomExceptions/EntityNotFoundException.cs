using System;

namespace Cm.Api.Common.CustomExceptions
{
    /// <summary>
    /// Exception in case when entity was not found
    /// </summary>
    public class EntityNotFoundException: ApplicationException
    {
        /// <summary>
        /// Creates the new instance of the class
        /// </summary>
        /// <param name="message"></param>
        public EntityNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates the new instance of the class with default message
        /// </summary>
        public EntityNotFoundException() : base("Entity was not found")
        {
        }
    }
}
