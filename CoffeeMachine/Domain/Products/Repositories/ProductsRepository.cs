using Domain.Common.Dal;

namespace Domain.Products.Repositories
{
    public class ProductsRepository: Repository<Product>
    {
        public ProductsRepository(IDbConnectionProvider dbConnectionProvider) : base(dbConnectionProvider)
        {
        }
    }
}
