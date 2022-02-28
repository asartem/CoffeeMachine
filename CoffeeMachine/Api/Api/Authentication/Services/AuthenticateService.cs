using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cm.Api.Application.Settings;
using Cm.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Cm.Api.Api.Authentication.Services
{
    /// <summary>
    /// Service for authentication
    /// </summary>
    public class AuthenticateService : IAuthenticateService
    {
        /// <summary>
        /// Users repository
        /// </summary>
        public IUsersRepository UsersRepository { get; }

        /// <summary>
        /// Application settings with secret key
        /// </summary>
        public AppSettings AppSettings { get; }

        /// <summary>
        /// Creates instance of the service
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="usersRepository"></param>
        public AuthenticateService(IOptions<AppSettings> appSettings, IUsersRepository usersRepository)
        {
            UsersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            AppSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings));
        }

        /// <summary>
        /// Get toke by login and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<string> GetToken(string userName, string password)
        {
            User user = (await UsersRepository.FindAsync(x => x.Name == userName))
                .SingleOrDefault();

            if (user == null)
            {
                return null;
            }

            if (user.Password != password) // TODO: replace with  passwords' hashes comparision
            {
                return null;
            }


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Name),
                        new Claim(ClaimTypes.Role, user.Role.Name),
                    }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            var resultToken = tokenHandler.WriteToken(token);
            return resultToken;

        }


    }
}