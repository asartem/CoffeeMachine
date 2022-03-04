using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cm.Api.Api.Products.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Cm.Tests.Api.Products.ProductsControllerClassTests
{
    [TestFixture]
    public class GetProductsByIdTests : ApiTestsBase
    {
        protected HttpClient TestClient { get; set; }
        protected IServiceProvider ServiceProvider;
        private const int userId = 1;
        [OneTimeSetUp]
        public virtual void OneTimeSetup()
        {
            ServiceProvider = TestDataServiceCollection.BuildServiceProvider();
        }

        [SetUp]
        public void SetupFixture()
        {
            TestClient = CreateClientWithToken(userId);
        }


        [Test]
        [Description("Should return product with correct mapping")]
        public async Task CorrectId_Product()
        {
            // Arrange
            int teaProductId = 1;

            // Act
            var response = await TestClient.GetAsync($"/products/{teaProductId}");
            
            // Assert
            var resultAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductDto>(resultAsString);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(teaProductId, result.Id);
            Assert.AreEqual("Tea", result.Name);
            Assert.AreEqual(5, result.Price);
            Assert.AreEqual(100, result.Quantity);

        }


        [Test]
        [Description("Should return no content for non existing product")]
        public async Task NonExistingId_NotContent()
        {
            // Arrange
            int teaProductId = 9999;

            // Act
            var response = await TestClient.GetAsync($"/products/{teaProductId}");
            
            // Assert

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        }

        [Test]
        [Description("Should return not found for invalid id")]
        public async Task InvalidId_NotFound()
        {
            // Arrange
            int teaProductId = -1;

            // Act
            var response = await TestClient.GetAsync($"/products/{teaProductId}");
            
            // Assert

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        }


        [Test]
        [Description("Should return product for anonymous user")]
        public async Task AnonymousUser_Product()
        {
            // Arrange
            int teaProductId = 1;
            var invalidClient = CreateClientWithToken(99);
            invalidClient.DefaultRequestHeaders.Authorization = null;
            
            // Act
            var response = await invalidClient.GetAsync($"/products/{teaProductId}");
            
            // Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
    }
}
