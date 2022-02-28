using System;
using System.Linq;
using System.Threading.Tasks;
using Cm.Domain.Products;
using Cm.Domain.Users;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Cm.Tests.Domain.Products.Repositories.ProductsRepositoryClassTests
{

    [TestFixture]
    public class RemoveMethodTests : AllTestsSetup
    {

        const string TestProductName = "TestProduct";
        protected IServiceProvider ServiceProvider;

        public RemoveMethodTests()
        {
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        [OneTimeSetUp]
        public void Setup()
        {
            // Create test data here
        }

        [TearDown]
        public void Cleanup()
        {
            var repository = ServiceProvider.GetService<IProductsRepository>();
            var allProductsAfterSave = repository.GetAll();
            foreach (var product in allProductsAfterSave)
            {
                if (product.Id > 5)
                {
                    repository.Remove(product);
                }
            }
        }


        [Test]
        public async Task ExistingProduct_Removed()
        {

            var repository = ServiceProvider.GetService<IProductsRepository>();
            var usersRepository = ServiceProvider.GetService<IUsersRepository>();

            var allUsers = await usersRepository.GetAllAsync();
            var firstUser = allUsers.First();

            Product product = new Product(TestProductName, firstUser, 5, 10);
            await repository.AddAsync(product);
            var allProductsAfterSave = await repository.GetAllAsync();
            var createdItem = allProductsAfterSave.FirstOrDefault(x => x.Name == product.Name);

            repository.Remove(createdItem);

            allProductsAfterSave = await repository.GetAllAsync();
            var removedItem = allProductsAfterSave.FirstOrDefault(x => x.Name == product.Name);

            Assert.IsNotNull(createdItem);
            Assert.IsNull(removedItem);
            

        }

        [Test]
        public async Task Id_Removed()
        {

            var repository = ServiceProvider.GetService<IProductsRepository>();
            var usersRepository = ServiceProvider.GetService<IUsersRepository>();

            var allUsers = await usersRepository.GetAllAsync();
            var firstUser = allUsers.First();

            Product product = new Product(TestProductName, firstUser, 5, 10);
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
