using System;
using System.ComponentModel.DataAnnotations;
using Cm.Domain.Deposits.Specifications;

namespace Cm.Api.Api.Deposit.Models
{
    /// <summary>
    /// Validates coins
    /// </summary>
    public class CoinsValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Coins validation algorithm
        /// </summary>
        private readonly IValidCoinsSpecification specification;

        /// <summary>
        /// Create instance of the validation attribute
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public CoinsValidationAttribute()
        {
            specification = new ValidCoinsSpecification();
        }

        /// <summary>
        /// Returns Success in case of valid value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int? amount = (int?)value;

            if (amount != null && specification.IsSatisfiedBy(amount.Value) == false)
            {
                string errorMessage = "Only coins 5,10,20,50 and 100 are allowed";
                return new ValidationResult(errorMessage);
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }

}
