using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cm.Domain.Users;
using Cm.Domain.Users.Roles;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Cm.Tests.Api.Deposit.DepositsControllerClassTests
{
    [TestFixture]
    public class ResetDepositAsyncTests : ApiTestsBase
    {
        protected HttpClient TestClientBuyer { get; set; }
        protected IServiceProvider ServiceProvider;
        private User buyer;
        private IUsersRepository usersRepository;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            ServiceProvider = TestDataServiceCollection.BuildServiceProvider();
            usersRepository = ServiceProvider.GetService<IUsersRepository>();
        }


        [SetUp]
        public async Task SetupFixture()
        {

            buyer = (await usersRepository.FindAsync(x => x.Role.Name == UserRoles.Buyer)).First();
            buyer.Deposit = 150;
            await usersRepository.AddAsync(buyer);

            TestClientBuyer = CreateClientWithToken(buyer);
        }

        [Description("Should set deposit to zero")]
        public async Task RestForUser_ZeroDeposit(int deposit)
        {
            // Arrange
            // Act
            var response = await TestClientBuyer.PutAsync("/deposits/reset", null);

            var resultAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<int>(resultAsString);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(0, result);
        }

        [Description("Should not allow to reset for anonymous")]
        public async Task RestForSeller_Forbidden(int deposit)
        {
            // Arrange
            // Act
            HttpClient client = Factory.CreateClient();
            var response = await client.PutAsync("/deposits/reset", null);
            
            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

    }
}
