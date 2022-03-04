using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cm.Api.Api.Buy.Models;
using Cm.Api.Api.Products.Models;
using Cm.Domain.Products;
using Cm.Domain.Users;
using Cm.Domain.Users.Roles;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Cm.Tests.Api.Buy.BuysControllerClassTests
{
    [TestFixture]
    public class CreateOrderTests : ApiTestsBase
    {
        protected HttpClient TestClientSeller { get; set; }
        protected HttpClient TestClientBuyer { get; set; }
        protected IServiceProvider ServiceProvider;
        private User seller;
        private User buyer;
        private ProductDto createdProduct1;
        private ProductDto createdProduct2;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            ServiceProvider = TestDataServiceCollection.BuildServiceProvider();
            var repository = ServiceProvider.GetService<IUsersRepository>();

            buyer = (await repository.FindAsync(x => x.Role.Name == UserRoles.Seller)).First();
            buyer.Deposit = 150;
            await repository.AddAsync(buyer);
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

            createdProduct1 = await CreateProductForUpdate("TestProduct1");
            createdProduct2 = await CreateProductForUpdate("TestProduct2");
        }


        [Test]
        [Description("Should calculate qty correctly")]
        public async Task ProductsAndQty_CorrectQty()
        {
            // Arrange

            const int product1SoldQty = 2;
            const int product2SoldQty = 3;
            var model = new CreateOrderDto
            {
                ProductsAndQuantity = new Dictionary<int, int>()
                {
                    {createdProduct1.Id, product1SoldQty},
                    {createdProduct2.Id, product2SoldQty}
                }
            };
            StringContent httpContent = ContentHelper.GetStringContent(model);

            // Act
            var response = await TestClientBuyer.PostAsync("/Buys", httpContent);
         

            // Assert
            var resultAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OrderDto>(resultAsString);

            var purchasedProduct1 = result.PurchasedItems.First(x => x.Id == createdProduct1.Id);
            var purchasedProduct2 = result.PurchasedItems.First(x => x.Id == createdProduct2.Id);


            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(product1SoldQty, purchasedProduct1.Quantity);
            Assert.AreEqual(product2SoldQty, purchasedProduct2.Quantity);

        }


        [Test]
        [Description("Should calculate cost per product correctly")]
        public async Task ProductsAndQty_CorrectCost()
        {
            // Arrange

            const int product1SoldQty = 2;
            const int product2SoldQty = 3;
            var model = new CreateOrderDto
            {
                ProductsAndQuantity = new Dictionary<int, int>
                {
                    {createdProduct1.Id, product1SoldQty},
                    {createdProduct2.Id, product2SoldQty}
                }
            };
            StringContent httpContent = ContentHelper.GetStringContent(model);

            // Act
            var response = await TestClientBuyer.PostAsync("/buys", httpContent);


            // Assert
            var resultAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OrderDto>(resultAsString);

            var purchasedProduct1 = result.PurchasedItems.First(x => x.Id == createdProduct1.Id);
            var purchasedProduct2 = result.PurchasedItems.First(x => x.Id == createdProduct2.Id);


            Assert.AreEqual(product1SoldQty * createdProduct1.Price, purchasedProduct1.Cost);
            Assert.AreEqual(product2SoldQty * createdProduct2.Price, purchasedProduct2.Cost);

            var expectedTotalAmount = product1SoldQty * createdProduct1.Price + product2SoldQty * createdProduct2.Price;
            Assert.AreEqual(expectedTotalAmount, result.TotalAmount);


        }


        [Test]
        [Description("Should get change with 5,10,20,50 or 100 coins only")]
        public async Task ProductsAndQty_Change()
        {
            // Arrange

            const int product1SoldQty = 2;
            const int product2SoldQty = 3;
            var model = new CreateOrderDto
            {
                ProductsAndQuantity = new Dictionary<int, int>
                {
                    {createdProduct1.Id, product1SoldQty},
                    {createdProduct2.Id, product2SoldQty}
                }
            };
            StringContent httpContent = ContentHelper.GetStringContent(model);

            // Act
            var response = await TestClientBuyer.PostAsync("/buys", httpContent);


            // Assert
            var resultAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OrderDto>(resultAsString);

            var validCoins = new int[] { 5, 10, 20, 50, 100 };
            foreach (int coin in result.Change)
            {
                Assert.True(validCoins.Any(x => x == coin));

            }

        }


        [Test]
        [Description("Should allow seller to buy products")]
        public async Task Seller_CanBuy()
        {
            // Arrange

            const int product1SoldQty = 2;
            const int product2SoldQty = 3;
            var model = new CreateOrderDto
            {
                ProductsAndQuantity = new Dictionary<int, int>
                {
                    {createdProduct1.Id, product1SoldQty},
                    {createdProduct2.Id, product2SoldQty}
                }
            };
            StringContent httpContent = ContentHelper.GetStringContent(model);

            // Act
            var response = await TestClientSeller.PostAsync("/buys", httpContent);


            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }


        private async Task<ProductDto> CreateProductForUpdate(string name)
        {
            var createModel = new CreateProductDto()
            {
                Name = name,
                Price = 15,
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
