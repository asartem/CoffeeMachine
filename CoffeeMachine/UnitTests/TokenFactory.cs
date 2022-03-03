using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Cm.Domain.Users;
using Cm.Domain.Users.Roles;
using Microsoft.IdentityModel.Tokens;

namespace Cm.Tests
{
    public class TokenFactory
    {
        public GenericPrincipal CreateUserForContext(int userId, string userRole = UserRoles.Seller)
        {
            User user = CreateDefaultUser(userId, userRole);

            ClaimsIdentity identity = CreateClaimsIdentity(user);
            var principal = new GenericPrincipal(identity, new[] { userRole });

            return principal;
        }


        public string GenerateStandardToken(string secret,
            int userId,
            string userRole = UserRoles.Seller,
            TimeSpan? expiresIn = null)
        {
            User user = CreateDefaultUser(userId, userRole);
            var tokenString = GenerateStandardToken(secret, user, expiresIn);
            return tokenString;
        }

        public string GenerateStandardToken(string secret,
            User user,
            TimeSpan? expiresIn = null)
        {
            expiresIn ??= TimeSpan.FromSeconds(200);

            ClaimsIdentity identity = CreateClaimsIdentity(user);
            var tokenString = IssuerToken(secret, expiresIn, identity);
            return tokenString;
        }


        private static User CreateDefaultUser(int userId, string userRole)
        {
            var user = new User("Test Seller",
                "1234",
                100,
                new UserRole(userRole == UserRoles.Seller ? 1 : 2, userRole))
            {
                Id = userId
            };
            return user;
        }


        private static ClaimsIdentity CreateClaimsIdentity(User user)
        {
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Name));
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.Name));
            return identity;
        }

        private static string IssuerToken(string secret, TimeSpan? expiresIn, ClaimsIdentity identity)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                //Expires = new DateTime(expiresIn?.Ticks ?? TimeSpan.FromMinutes(1).Ticks),
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