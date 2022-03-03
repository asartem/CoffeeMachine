using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cm.Domain.Products;
using Cm.Domain.Users;
using Cm.Domain.Users.Roles;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Cm.Tests.Domain.Products.Repositories.ProductsRepositoryClassTests
{
    [TestFixture]
    public class GetMethodTests : AllTestsSetup
    {

        private IList<Product> existingProducts;
        protected IServiceProvider ServiceProvider;
        private User seller;

        public GetMethodTests()
        {
            ServiceProvider = ServiceCollection.BuildServiceProvider();

        }

        [OneTimeSetUp]
        public async Task Setup()
        {
            var repository = ServiceProvider.GetService<IProductsRepository>();
            existingProducts = (await repository.GetAllAsync()).ToList();

            var userRepository = ServiceProvider.GetService<IUsersRepository>();
            seller = (await userRepository.FindAsync(x => x.Role.Name == UserRoles.Seller)).First();

            // Create test data here with data framework
        }

        [Test]
        public async Task ProductId_ExpectedProduct()
        {

            var repository = ServiceProvider.GetService<IProductsRepository>();

            var expectedProduct = existingProducts.First();

            var result = await repository.GetAsync(expectedProduct.Id);
            Assert.AreEqual(expectedProduct.Id, result.Id);
            Assert.AreEqual(expectedProduct.Name, result.Name);
            Assert.IsNotNull(result.Name);

        }

        [Test]
        public async Task Empty_AllProducts()
        {

            var repository = ServiceProvider.GetService<IProductsRepository>();
            var result = await repository.GetAllAsync();
            Assert.AreEqual(existingProducts.Count, result.Count());

        }

    }
}
