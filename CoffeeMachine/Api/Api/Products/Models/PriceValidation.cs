using System.ComponentModel.DataAnnotations;

namespace Cm.Api.Api.Products.Models
{
    /// <summary>
    /// Validates coins
    /// </summary>
    public class PriceValidation : ValidationAttribute
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

            if (amount != null
                && (amount < 0
                || amount % 5 != 0))
            {
                string errorMessage = "Price should be multiple 5 and not negative";

                return new ValidationResult(errorMessage);
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}