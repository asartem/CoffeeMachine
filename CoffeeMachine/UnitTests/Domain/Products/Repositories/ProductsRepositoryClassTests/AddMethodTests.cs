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
    public class AddMethodTests : AllTestsSetup
    {

        const string TestProductName = "TestProduct";
        protected IServiceProvider ServiceProvider;
        private User seller;
        public AddMethodTests()
        {
            ServiceProvider = ServiceCollection.BuildServiceProvider();

        }

        [OneTimeSetUp]
        public async Task Setup()
        {
            var usersRepository = ServiceProvider.GetService<IUsersRepository>();
            seller = (await usersRepository.FindAsync(x => x.Role.Name == UserRoles.Seller)).First();
            
        }

        [TearDown]
        public async Task Cleanup()
        {
            var repository = ServiceProvider.GetService<IProductsRepository>();
            var allProductsAfterSave = await repository.GetAllAsync();
            const int predefinedProductsNumber = 5;
            foreach (var product in allProductsAfterSave)
            {
                if (product.Id > predefinedProductsNumber)
                {
                    await repository.RemoveAsync(product);
                }
            }

        }


        [Test]
        public async Task NewProduct_SavedInDb()
        {

            var repository = ServiceProvider.GetService<IProductsRepository>();

            Product product = new Product(TestProductName, seller, 5, 10);
            await repository.AddAsync(product);
            var allProductsAfterSave = await repository.GetAllAsync();

            Assert.IsTrue(allProductsAfterSave.Any(x => x.Name == product.Name
                                                        && x.Id > 0));
        }

        [Test]
        public async Task ExistingProduct_UpdatedInDb()
        {

            var repository = ServiceProvider.GetService<IProductsRepository>();
            
            Product product = new Product(TestProductName, seller, 5, 10);
            await repository.AddAsync(product);
            var allProductsAfterSave = await repository.GetAllAsync();

            var justCreateProduct = allProductsAfterSave.FirstOrDefault(x => x.Name == TestProductName);

            justCreateProduct.Name = "UpdatedTest";

            await repository.AddAsync(justCreateProduct);

            allProductsAfterSave = await repository.GetAllAsync();
            Assert.IsTrue(allProductsAfterSave.Any(x => x.Name == justCreateProduct.Name
                                                        && x.Id == justCreateProduct.Id));

        }


    }
}
