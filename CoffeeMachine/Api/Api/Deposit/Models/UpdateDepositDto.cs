using System.ComponentModel.DataAnnotations;

namespace Cm.Api.Api.Deposit.Models
{
    /// <summary>
    /// Model for deposit update
    /// </summary>
    public class UpdateDepositDto
    {
        /// <summary>
        /// Amount of money
        /// </summary>
        [CoinsValidation]
        [Range(5, int.MaxValue, ErrorMessage = "At least one coin is required")]
        public int Deposit { get; set; }
    }
}
