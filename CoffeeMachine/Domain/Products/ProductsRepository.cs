using Cm.Domain.Common.Dal;

namespace Cm.Domain.Products
{
    /// <summary>
    /// Products repository
    /// </summary>
    public class ProductsRepository : Repository<Product>, IProductsRepository
    {
        /// <summary>
        /// Creates the instance of the class
        /// </summary>
        /// <param name="context"></param>
        public ProductsRepository(ApplicationContext context) : base(context)
        {
        }
    }


}
