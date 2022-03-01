using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Cm.Api.Api.Users.Models
{
    /// <summary>
    /// Validates coins
    /// </summary>
    public class CoinsValidation : ValidationAttribute
    {
        /// <summary>
        /// Returns Success in case of valid value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int? amount = (int?)value;

            var allowedCoins = new[] { 5, 10, 20, 50, 100 };
            if (amount != null
                && (amount < 1
                || allowedCoins.All(x => x != amount)))
            {
                string errorMessage = "Only coins 5,10,15,25,50 and 100 are allowed";
                return new ValidationResult(errorMessage);
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
