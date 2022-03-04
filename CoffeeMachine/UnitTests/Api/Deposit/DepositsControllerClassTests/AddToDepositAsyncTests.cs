using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cm.Api.Api.Deposit.Models;
using Cm.Domain.Users;
using Cm.Domain.Users.Roles;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Cm.Tests.Api.Deposit.DepositsControllerClassTests
{
    [TestFixture]
    public class AddToDepositAsyncTests : ApiTestsBase
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

        [TestCase(5)]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(50)]
        [TestCase(100)]
        [Description("Should add money to deposit")]
        public async Task ValidCoins_Added(int deposit)
        {
            // Arrange
            var model = new UpdateDepositDto
            {
                Deposit = deposit
            };
            StringContent httpContent = ContentHelper.GetStringContent(model);

            // Act
            var response = await TestClientBuyer.PutAsync("/deposits", httpContent);

            var resultAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<int>(resultAsString);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(buyer.Deposit + deposit, result);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(4)]
        [TestCase(6)]
        [Description("Should not add invalid coins")]
        public async Task InvalidCoins_NotAdded(int deposit)
        {
            // Arrange
            var model = new UpdateDepositDto
            {
                Deposit = deposit
            };
            StringContent httpContent = ContentHelper.GetStringContent(model);

            // Act
            var response = await TestClientBuyer.PutAsync("/deposits", httpContent);

            await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        [Description("Should not allow to update for anonymous")]
        public async Task Anonymous_Unauthorized()
        {
            // Arrange
            var model = new UpdateDepositDto()
            {
                Deposit = 5
            };
            StringContent httpContent = ContentHelper.GetStringContent(model);
            HttpClient client = Factory.CreateClient();
            // Act
            var response = await client.PutAsync("/deposits", httpContent);


            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
