using System.ComponentModel.DataAnnotations;
using Cm.Api.Api.Products.Models;
using static System.Int32;

namespace Cm.Api.Api.Users.Models
{
    /// <summary>
    /// Base class for User DTOs
    /// </summary>
    public abstract class UserDtoBase
    {
        /// <summary>
        /// Deposit
        /// </summary>
        [CoinsValidation]
        public int? Deposit { get; set; }

    }

}