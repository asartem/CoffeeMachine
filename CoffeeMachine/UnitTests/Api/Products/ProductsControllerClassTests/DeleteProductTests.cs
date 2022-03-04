﻿using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Cm.Tests.Api.Products.ProductsControllerClassTests
{
    [TestFixture]
    public class CreateOrderTests : ProductsControllerTestsBase
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
        
    }
}
