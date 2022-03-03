using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cm.Api.Api.Products.Models;
using Cm.Domain.Products;
using Cm.Domain.Users;
using Cm.Domain.Users.Roles;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Cm.Tests.Api.Products.ProductsControllerClassTests
{
    [TestFixture]
    public class DeleteProductTests : ApiTestsBase
    {
        protected HttpClient TestClientSeller { get; set; }
        protected HttpClient TestClientBuyer { get; set; }
        protected IServiceProvider ServiceProvider;
        private User buyer;
        private User seller;
        private ProductDto createdProduct;
        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            ServiceProvider = TestDataServiceCollection.BuildServiceProvider();
            var repository = ServiceProvider.GetService<IUsersRepository>();
            buyer = (await repository.FindAsync(x => x.Role.Name == UserRoles.Buyer)).First();
            seller = (await repository.FindAsync(x => x.Role.Name == UserRoles.Seller)).First();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await CleanUpProducts();
        }

        [SetUp]
        public async Task SetupFixture()
        {
            await CleanUpProducts();

            TestClientSeller = CreateClientWithToken(seller);
            TestClientBuyer = CreateClientWithToken(buyer);

            createdProduct = await CreateProductForUpdate();
        }


        [Test]
        [Description("Should remove existing product by seller")]
        public async Task ProductId_Removed()
        {
            // Arrange
            // Act
            var response = await TestClientSeller.DeleteAsync($"/products/{createdProduct.Id}");


            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        [Description("Should return NoContent for non existing product")]
        public async Task NonExistingId_NoContent()
        {
            // Arrange
            await TestClientSeller.DeleteAsync($"/products/{createdProduct.Id}");

            // Act
            var response = await TestClientSeller.DeleteAsync($"/products/{createdProduct.Id}");


            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        [Description("Should not remove existing product by buyer")]
        public async Task ProductId_Forbidden()
        {
            // Arrange
            // Act
            var response = await TestClientBuyer.DeleteAsync($"/products/{createdProduct.Id}");


            // Assert
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Test]
        [Description("Should not remove existing product by anonymous")]
        public async Task Anonymous_Unauthorized()
        {
            // Arrange
            // Act
            var client = Factory.CreateClient();
            var response = await client.DeleteAsync($"/products/{createdProduct.Id}");


            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        
        private async Task<ProductDto> CreateProductForUpdate()
        {
            var createModel = new CreateProductDto()
            {
                Name = "TestProduct1",
                Price = 100,
                Quantity = 10
            };

            StringContent httpContent = ContentHelper.GetStringContent(createModel);
            var response = await TestClientSeller.PostAsync("/products", httpContent);

            var resultAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductDto>(resultAsString);
            return result;
        }

        private async Task CleanUpProducts()
        {
            var repository = ServiceProvider.GetService<IProductsRepository>();
            var products = await repository.FindAsync(x => x.Name.Contains("Test"));

            foreach (var entity in products)
            {
                await repository.RemoveAsync(entity);
            }
        }
    }
}
