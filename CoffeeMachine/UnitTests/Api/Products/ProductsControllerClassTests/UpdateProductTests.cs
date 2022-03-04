using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cm.Api.Api.Products.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Cm.Tests.Api.Products.ProductsControllerClassTests
{
    [TestFixture]
    public class UpdateProductTests : ProductsControllerTestsBase
    {

        [SetUp]
        public async Task SetupFixture()
        {
            await CleanUpProducts();

            TestClientSeller = CreateClientWithToken(seller);
            TestClientBuyer = CreateClientWithToken(buyer);

            createdProduct = await CreateProductForUpdate();
        }


        [Test]
        [Description("Should update existing product")]
        public async Task ProductWithChanges_UpdatedProduct()
        {
            // Arrange
            var model = new UpdateProductDto()
            {
                Name = "TestProduct2",
                Price = 10,
                Quantity = 5
            };
            StringContent httpContent = ContentHelper.GetStringContent(model);

            // Act
            var response = await TestClientSeller.PutAsync($"/products/{createdProduct.Id}", httpContent);


            // Assert
            var resultAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductDto>(resultAsString);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(model.Name, result.Name);
            Assert.AreEqual(model.Price, result.Price);
            Assert.AreEqual(model.Quantity, result.Quantity);
            Assert.AreEqual(result.Id, createdProduct.Id);
        }


        [TestCase(5)]
        [TestCase(10)]
        [TestCase(15)]
        [TestCase(20)]
        [TestCase(50)]
        [TestCase(60)]
        [TestCase(110)]
        [Description("Should update product with price multiple to 5 only")]
        public async Task PriceMult5_UpdatedProduct(int price)
        {
            // Arrange
            var model = new UpdateProductDto()
            {
                Name = "TestProduct2",
                Price = price,
                Quantity = 5
            };
            StringContent httpContent = ContentHelper.GetStringContent(model);

            // Act
            var response = await TestClientSeller.PutAsync($"/products/{createdProduct.Id}", httpContent);

            // Assert
            var resultAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductDto>(resultAsString);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(model.Price, result.Price);
        }


        [Test]
        [Description("Should return not allowed for buyer")]
        public async Task Buyer_CantUpdateProduct()
        {
            // Arrange
            var model = new UpdateProductDto()
            {
                Name = "TestProduct2",
                Price = 10,
                Quantity = 5
            };
            StringContent httpContent = ContentHelper.GetStringContent(model);

            // Act
            var response = await TestClientBuyer.PutAsync($"/products/{createdProduct.Id}", httpContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Test]
        [Description("Should return Unauthorized for anonymous")]
        public async Task AnonymousUpdate_Unauthorized()
        {
            // Arrange
            var model = new UpdateProductDto()
            {
                Name = "TestProduct2",
                Price = 10,
                Quantity = 5
            };
            StringContent httpContent = ContentHelper.GetStringContent(model);

            // Act
            var client = Factory.CreateClient();
            var response = await client.PutAsync($"/products/{createdProduct.Id}", httpContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }


        [Test]
        [Description("Should ignore if empty Name")]
        public async Task InvalidName_CantUpdateProduct()
        {
            // Arrange
            var model = new UpdateProductDto()
            {
                Name = "",
                Price = 10,
                Quantity = 5
            };
            StringContent httpContent = ContentHelper.GetStringContent(model);

            // Act
            var response = await TestClientSeller.PutAsync($"/products/{createdProduct.Id}", httpContent);

            var resultAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductDto>(resultAsString);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(createdProduct.Name, result.Name);
        }

        [TestCase(-1)]
        [TestCase(1)]
        [TestCase(4)]
        [TestCase(6)]
        [Description("Should not update with invalid price not mult on 5")]
        public async Task InvalidPrice_CantCrateProduct(int price)
        {
            // Arrange
            var model = new UpdateProductDto()
            {
                Name = "TestProduct2",
                Price = price,
                Quantity = 5
            };
            StringContent httpContent = ContentHelper.GetStringContent(model);

            // Act
            var response = await TestClientSeller.PutAsync($"/products/{createdProduct.Id}", httpContent);


            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [TestCase(-1)]
        [Description("Should not update with invalid quantity")]
        public async Task InvalidQty_CantCrateProduct(int qty)
        {
            // Arrange
            var model = new UpdateProductDto()
            {
                Name = "TestProduct2",
                Price = 10,
                Quantity = qty
            };
            StringContent httpContent = ContentHelper.GetStringContent(model);

            // Act
            var response = await TestClientSeller.PutAsync($"/products/{createdProduct.Id}", httpContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
