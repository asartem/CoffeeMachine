using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cm.Domain.Products;
using Cm.Domain.Purchases.Exceptions;
using Cm.Domain.Purchases.Models;
using Cm.Domain.Users;

namespace Cm.Domain.Purchases
{
    /// <summary>
    /// Service which makes a purchase
    /// </summary>
    public class PurchaseService : IPurchaseService
    {
        /// <summary>
        /// Repository with user
        /// </summary>
        public IUsersRepository UsersRepository { get; }

        /// <summary>
        /// Products repository
        /// </summary>
        public IProductsRepository ProductsRepository { get; }

        public PurchaseService(IUsersRepository usersRepository, IProductsRepository productsRepository)
        {
            UsersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            ProductsRepository = productsRepository ?? throw new ArgumentNullException(nameof(productsRepository));
        }

        /// <summary>
        /// Make a purchase
        /// </summary>
        /// <returns></returns>
        public async Task<Order> CreateOrderAsync(IDictionary<int, int> productIdsAndQty, int buyerId)
        {

            IDictionary<Product, int> orderItems = await CreateCollectionOfProductsToBuy(productIdsAndQty);

            Order order = new Order(orderItems);
            User buyer = await UsersRepository.GetAsync(buyerId);
            if (buyer.Deposit < order.TotalCost)
            {
                throw new InsufficientFoundsException($"There is no enough founds ({buyer.Deposit}). {order.TotalCost} cents are required");
            }

            await QtyAndCostRecalculation(orderItems, buyer);

            order.SetChange(buyer.Deposit); // TODO: note clear: should we reset deposit to zero and return change or just return possible change amount

            return order;

        }

        /// <summary>
        /// Create the collection of products and qty.
        /// </summary>
        /// <param name="productIdsAndQty"></param>
        /// <returns></returns>
        private async Task<IDictionary<Product, int>> CreateCollectionOfProductsToBuy(IDictionary<int, int> productIdsAndQty)
        {
            IDictionary<Product, int> orderItems = new Dictionary<Product, int>();

            IList<int> productIds = productIdsAndQty.Select(x => x.Key).ToList();

            ICollection<Product> products = (await ProductsRepository.FindAsync(x => productIds.Contains(x.Id))).ToList();

            ValidateProductsQtyAndAvailability(productIdsAndQty, products);

            foreach (KeyValuePair<int, int> keyValuePair in productIdsAndQty)
            {
                int productId = keyValuePair.Key;
                int qty = keyValuePair.Value;

                Product product = products.First(x => x.Id == productId);
                orderItems.Add(product, qty);
            }

            return orderItems;
        }


        /// <summary>
        /// Reduces qty of products in DB and purchased amount on deposit
        /// </summary>
        /// <param name="orderItems"></param>
        /// <param name="buyer"></param>
        /// <returns></returns>
        private async Task QtyAndCostRecalculation(IDictionary<Product, int> orderItems, User buyer)
        {
            //NOTE: refactor. Replace with unit of work
            foreach (KeyValuePair<Product, int> orderItem in orderItems)
            {
                Product product = orderItem.Key;
                int qty = orderItem.Value;
                int itemCost = product.Price * qty;

                product.ReduceQty(qty);
                buyer.ReduceDeposit(itemCost); // TODO: not clear, should I increase seller deposit or not.

                ProductsRepository.AddToContext(product);
            }

            UsersRepository.AddToContext(buyer);
            await UsersRepository.SaveContextAsync();
        }

        /// <summary>
        /// Validates that there are enough products to make a purchase
        /// </summary>
        /// <param name="orderItems"></param>
        /// <param name="products"></param>
        private static void ValidateProductsQtyAndAvailability(IDictionary<int, int> orderItems, ICollection<Product> products)
        {
            foreach (var orderItem in orderItems)
            {
                int productId = orderItem.Key;
                int expectedQty = orderItem.Value;
                Product product = products.FirstOrDefault(x => x.Id == productId);
                if (product == null)
                {
                    throw new EntityNotFoundException($"You try to buy not existing product {productId}");
                }

                if (product.Qty < expectedQty)
                {
                    throw new ProductIsOutOfStockException($"Only {product.Qty} items left for product {product.Name}");
                }
            }
        }

        /// <summary>
        /// Flag for dispose
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Dispose context
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    UsersRepository.Dispose();
                    ProductsRepository.Dispose();
                }
            }
            disposed = true;

        }

        /// <summary>
        /// Cleanup service
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
