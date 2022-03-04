using System;
using System.Linq;
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
    public class ProductsControllerTestsBase : ApiTestsBase
    {
        protected HttpClient TestClientSeller { get; set; }
        protected HttpClient TestClientBuyer { get; set; }
        protected IServiceProvider ServiceProvider;
        protected User buyer;
        protected User seller;
        protected ProductDto createdProduct;

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

        protected async Task<ProductDto> CreateProductForUpdate()
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

        protected async Task CleanUpProducts()
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