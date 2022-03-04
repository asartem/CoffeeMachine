using System.Threading.Tasks;

namespace Cm.Api.Api.Authentication.Services
{
    /// <summary>
    /// Authentication service
    /// </summary>
    public interface IAuthenticateService
    {
        /// <summary>
        /// Returns token for valid login and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<string> GetToken(string userName, string password);
    }
}