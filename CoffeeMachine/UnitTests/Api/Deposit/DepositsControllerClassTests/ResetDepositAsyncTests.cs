using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cm.Domain.Users;
using Cm.Domain.Users.Roles;
using Microsoft.Extensions.DependencyInjection;
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
        public void OneTimeSetup()
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

        [Test]
        [Description("Should set deposit to zero for buyer")]
        public async Task RestForBoyer_ZeroDeposit()
        {
            // Arrange
            // Act
            var response = await TestClientBuyer.PutAsync("/deposits/reset", null);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        [Description("Should not allow to reset for seller")]
        public async Task RestForSeller_Forbidden()
        {
            // Arrange
            // Act
            var seller = (await usersRepository.FindAsync(x => x.Role.Name == UserRoles.Seller)).First();
            var testClientSeller = CreateClientWithToken(seller);
            var response = await testClientSeller.PutAsync("/deposits/reset", null);
            
            // Assert
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Test]
        [Description("Should not allow to reset for anonymous")]
        public async Task RestForAnonymous_Unauthorized()
        {
            // Arrange
            // Act
            var client = Factory.CreateClient();
            var response = await client.PutAsync("/deposits/reset", null);
            
            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

    }
}
