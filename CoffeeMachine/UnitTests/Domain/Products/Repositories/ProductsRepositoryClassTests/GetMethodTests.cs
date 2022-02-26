using System.Collections.Generic;
using Domain.Common.Dal;
using Domain.Products.Repositories;
using NUnit.Framework;

namespace Tests.Domain.Products.Repositories.ProductsRepositoryClassTests
{
    [TestFixture]
    public class GetMethodTests
    {
        private string connectionString = "";
        private readonly ProductsRepository repository = new ProductsRepository(new DbConnectionProvider(""));
        private IList<Product> existingProducts;

        public GetMethodTests()
        {
            existingProducts = CreateTestProducts();
        }
        

        [Test]
        public void GetProduct()
        {
            

            repository.Get(1);
        }


        public IList<Product> CreateTestProducts()
        {
            IList<Product> products = new List<Product>();

            for (int i = 0; i < 10; i++)
            {
                Product nextProduct = new Product("Product #" + i);
                products.Add(nextProduct);


            }


            
            return products;
        }
    }
}
