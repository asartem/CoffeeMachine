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
    public class CreateProductTests : ApiTestsBase
    {
        protected HttpClient TestClientSeller { get; set; }
        protected HttpClient TestClientBuyer { get; set; }
        protected IServiceProvider ServiceProvider;
        private User buyer;
        private User seller;

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
        }


        [Test]
        [Description("Should create new product")]
        public async Task Product_CreatedNewProduct()
        {
            // Arrange
            var model = new CreateProductDto()
            {
                Name = "TestProduct1",
                Price = 100,
                Quantity = 10
            };

            StringContent httpContent = ContentHelper.GetStringContent(model);

            // Act
            var response = await TestClientSeller.PostAsync("/products", httpContent);

            // Assert
            var resultAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductDto>(resultAsString);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(model.Name, result.Name);
            Assert.AreEqual(model.Price, result.Price);
            Assert.AreEqual(model.Quantity, result.Quantity);
            Assert.Greater(result.Id, 0);
        }


        [TestCase(5)]
        [TestCase(10)]
        [TestCase(15)]
        [TestCase(20)]
        [TestCase(50)]
        [TestCase(60)]
        [TestCase(110)]
        [Description("Should create product with price multiple to to 5 only")]
        public async Task PriceMult5_CreatedNewProduct(int price)
        {
            // Arrange
            var model = new CreateProductDto()
            {
                Name = "TestProduct1",
                Price = price,
                Quantity = 10
            };

            StringContent httpContent = ContentHelper.GetStringContent(model);

            // Act
            var response = await TestClientSeller.PostAsync("/products", httpContent);

            // Assert
            var resultAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductDto>(resultAsString);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(model.Price, result.Price);
        }


        [Test]
        [Description("Should return not allowed for buyer")]
        public async Task Buyer_CantCrateProduct()
        {
            // Arrange
            var model = new CreateProductDto()
            {
                Name = "TestProduct1",
                Price = 100,
                Quantity = 10
            };

            StringContent httpContent = ContentHelper.GetStringContent(model);


            // Act
            var response = await TestClientBuyer.PostAsync("/products", httpContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Test]
        [Description("Should return Unauthorized for anonymous")]
        public async Task Anonymous_Unauthorized()
        {
            // Arrange
            var model = new CreateProductDto()
            {
                Name = "TestProduct1",
                Price = 100,
                Quantity = 10
            };

            StringContent httpContent = ContentHelper.GetStringContent(model);
            var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsync("/products", httpContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }


        [Test]
        [Description("Should not create the same product twice")]
        public async Task DuplicateProduct_Conflict()
        {
            // Arrange
            var model = new CreateProductDto()
            {
                Name = "TestProduct1",
                Price = 100,
                Quantity = 10
            };

            StringContent httpContent = ContentHelper.GetStringContent(model);
            var response1 = await TestClientSeller.PostAsync("/products", httpContent);

            // Act
            var response2 = await TestClientSeller.PostAsync("/products", httpContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.Conflict, response2.StatusCode);
        }


        [Test]
        [Description("Should not create with empty Name")]
        public async Task Invalid_CantCrateProduct()
        {
            // Arrange
            var model = new CreateProductDto()
            {
                Name = "",
                Price = 100,
                Quantity = 10
            };

            StringContent httpContent = ContentHelper.GetStringContent(model);


            // Act
            var response = await TestClientSeller.PostAsync("/products", httpContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestCase(-1)]
        [TestCase(1)]
        [TestCase(4)]
        [TestCase(6)]
        [Description("Should not create with invalid price not equal to 5")]
        public async Task InvalidPrice_CantCrateProduct(int price)
        {
            // Arrange
            var model = new CreateProductDto()
            {
                Name = "TestProduct1",
                Price = price,
                Quantity = 10
            };

            StringContent httpContent = ContentHelper.GetStringContent(model);


            // Act
            var response = await TestClientSeller.PostAsync("/products", httpContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [TestCase(-1)]
        [Description("Should not create with invalid quantity")]
        public async Task InvalidQty_CantCrateProduct(int qty)
        {
            // Arrange
            var model = new CreateProductDto()
            {
                Name = "TestProduct1",
                Price = 5,
                Quantity = qty
            };

            StringContent httpContent = ContentHelper.GetStringContent(model);


            // Act
            var response = await TestClientSeller.PostAsync("/products", httpContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
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
