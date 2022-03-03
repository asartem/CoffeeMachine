using System;
using System.Collections.Generic;
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
    public class GetAllProductsTests : ApiTestsBase
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
        [Description("Should return several product with correct mapping")]
        public async Task Empty_AllProducts()
        {
            // Arrange

            // Act
            var response = await TestClient.GetAsync("/products/");

            // Assert
            var resultAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IList<ProductDto>>(resultAsString);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.Greater(result.Count, 1);
            foreach (var dto in result)
            {
                Assert.Greater(dto.Id, 0);
                Assert.IsNotNull(dto.Name);
                Assert.GreaterOrEqual(dto.Price, 0);
                Assert.GreaterOrEqual(dto.Quantity, 0);
            }

        }


        [Test]
        [Description("Should return all products for anonymous user")]
        public async Task Anonymous_AllProducts()
        {
            // Arrange
            var invalidClient = CreateClientWithToken(99);
            invalidClient.DefaultRequestHeaders.Authorization = null;

            // Act
            var response = await invalidClient.GetAsync("/products");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var resultAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IList<ProductDto>>(resultAsString);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.Greater(result.Count, 1);

        }

        [Test]
        [Description("Should return not content for empty list of products")]
        public async Task NoProducts_NoContent()
        {
        }
    }
}
