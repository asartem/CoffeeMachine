using System;
using System.Linq;
using System.Threading.Tasks;
using Cm.Domain.Products;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Cm.Tests.Domain.Products.Repositories.ProductsRepositoryClassTests
{
    [TestFixture]
    public class AddMethodTests : AllTestsSetup
    {

        const string TestProductName = "TestProduct";
        protected IServiceProvider ServiceProvider;

        public AddMethodTests()
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
        public async Task NewProduct_SavedInDb()
        {

            var repository = ServiceProvider.GetService<IProductsRepository>();
            
            Product product = new Product(TestProductName, 5, 10);
            await repository.AddAsync(product);
            var allProductsAfterSave = await repository.GetAllAsync();

            Assert.IsTrue(allProductsAfterSave.Any(x => x.Name == product.Name
                                                        && x.Id > 0));
        }

        [Test]
        public async Task ExistingProduct_UpdatedInDb()
        {

            var repository = ServiceProvider.GetService<IProductsRepository>();
            

            Product product = new Product(TestProductName, 5, 10);
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
