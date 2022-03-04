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
    public class GetDepositAsyncTests : ApiTestsBase
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
        [Description("Should return deposit value for buyer")]
        public async Task User_Deposit()
        {
            // Arrange
            // Act
            var response = await TestClientBuyer.GetAsync("/deposits");

            var resultAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<int>(resultAsString);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(buyer.Deposit, result);
        }

        [Test]
        [Description("Should return unauthorized for anonymous")]
        public async Task Anonymous_Deposit()
        {
            // Arrange
            // Act
            HttpClient client = Factory.CreateClient();
            var response = await client.GetAsync("/deposits");
            

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

    }


}
