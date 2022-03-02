using System;
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
    public class RemoveMethodTests : AllTestsSetup
    {

        const string TestProductName = "TestProduct";
        protected IServiceProvider ServiceProvider;
        protected User seller;

        public RemoveMethodTests()
        {
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        [OneTimeSetUp]
        public async Task Setup()
        {
            var userRepository = ServiceProvider.GetService<IUsersRepository>();
            seller = (await userRepository.FindAsync(x => x.Role.Name == UserRoles.Seller)).First();

            // Create test data here
        }

        [TearDown]
        public async Task Cleanup()
        {
            var repository = ServiceProvider.GetService<IProductsRepository>();
            var allProductsAfterSave = (await repository.GetAllAsync());
            foreach (var product in allProductsAfterSave)
            {
                if (product.Id > 5)
                {
                    await repository.RemoveAsync(product);
                }
            }
        }


        [Test]
        public async Task ExistingProduct_Removed()
        {

            var repository = ServiceProvider.GetService<IProductsRepository>();

            Product product = new Product(TestProductName, seller, 5, 10);
            await repository.AddAsync(product);
            var allProductsAfterSave = await repository.GetAllAsync();
            var createdItem = allProductsAfterSave.FirstOrDefault(x => x.Name == product.Name);

            await repository.RemoveAsync(createdItem);

            allProductsAfterSave = await repository.GetAllAsync();
            var removedItem = allProductsAfterSave.FirstOrDefault(x => x.Name == product.Name);

            Assert.IsNotNull(createdItem);
            Assert.IsNull(removedItem);


        }

        [Test]
        public async Task Id_Removed()
        {

            var repository = ServiceProvider.GetService<IProductsRepository>();

            Product product = new Product(TestProductName, seller, 5, 10);
            await repository.AddAsync(product);
            var allProductsAfterSave = await repository.GetAllAsync();
            var createdItem = allProductsAfterSave.First(x => x.Name == product.Name);

            await repository.RemoveAsync(createdItem);

            allProductsAfterSave = await repository.GetAllAsync();
            var removedItem = allProductsAfterSave.FirstOrDefault(x => x.Name == product.Name);

            Assert.IsNotNull(createdItem);
            Assert.IsNull(removedItem);


        }



    }
}
