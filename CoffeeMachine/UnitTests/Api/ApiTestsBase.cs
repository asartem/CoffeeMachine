using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Cm.Domain.Users;
using Cm.Domain.Users.Roles;
using Cm.HostService;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Cm.Tests.Api
{
    public abstract class ApiTestsBase
    {
        protected WebApplicationFactory<Program> Factory = AllTestsSetup.Factory;
        protected ServiceCollection TestDataServiceCollection = AllTestsSetup.ServiceCollection;
        protected string JwtSecret = AllTestsSetup.JwtSecret;



        protected HttpClient CreateClientWithToken(int userId)
        {
            HttpClient client = Factory.CreateClient();

            var tokenFactory = new TokenFactory();
            var sellerToken = tokenFactory.GenerateStandardToken(JwtSecret, userId);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", sellerToken);
            return client;
        }

        protected HttpClient CreateClientWithToken(User user)
        {
            HttpClient client = Factory.CreateClient();

            var tokenFactory = new TokenFactory();
            var sellerToken = tokenFactory.GenerateStandardToken(JwtSecret, user);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", sellerToken);
            return client;
        }

        public static class ContentHelper
        {
            public static StringContent GetStringContent(object obj)
                => new StringContent(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");
        }

    }
}