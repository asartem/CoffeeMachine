using System.Security.Principal;

namespace Cm.Api.Api.Authentication.Models
{
    /// <summary>
    /// Extension methods for Identity
    /// </summary>
    public static class IdentityExtensions
    {
        /// <summary>
        /// Parses user id from Identity (token)
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static int Id(this IIdentity identity)
        {
            if (identity.IsAuthenticated 
                && int.TryParse(identity.Name, out int id))
            {
                return id;
            }

            return 0;
        }
    }
}
