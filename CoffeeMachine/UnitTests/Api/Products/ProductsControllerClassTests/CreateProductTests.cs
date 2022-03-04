using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cm.Api.Api.Products.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Cm.Tests.Api.Products.ProductsControllerClassTests
{
    [TestFixture]
    public class CreateProductTests : ProductsControllerTestsBase
    {
        
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
        
    }
}
