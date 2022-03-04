namespace Cm.Api.Api.Authentication.Models
{
    /// <summary>
    /// Authentication model
    /// </summary>
    public class UserAuthenticationDto
    {
        /// <summary>
        /// User's login
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        public string Password { get; set; }
    }
}