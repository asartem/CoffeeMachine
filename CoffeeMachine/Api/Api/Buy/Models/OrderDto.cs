using System;
using System.Collections.Generic;
using System.Linq;
using Cm.Domain.Products;
using Cm.Domain.Purchases.Models;

namespace Cm.Api.Api.Buy.Models
{
    /// <summary>
    /// Order result
    /// </summary>
    public class OrderDto
    {
        /// <summary>
        /// Purchased products
        /// </summary>
        public IList<SoldProductDto> PurchasedItems { get; set; } = new List<SoldProductDto>();

        /// <summary>
        /// Total amount
        /// </summary>
        public int TotalAmount
        {
            get { return PurchasedItems.Sum(x => x.Cost); }
        }

        /// <summary>
        /// Coins which were not spent
        /// </summary>
        public IList<int> Change;


        public OrderDto(){}

        /// <summary>
        /// Create instance of the class
        /// </summary>
        /// <param name="order"></param>
        public OrderDto(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            foreach (var soldProduct in order.ProductsAndQty)
            {
                Product product = soldProduct.Key;
                var itemDto = new SoldProductDto(product.Id, product.Name, product.Price, product.Qty);
                PurchasedItems.Add(itemDto);
            }

            Change = CreateChange(order.ChangeAmount);
        }

        /// <summary>
        /// Returns change as list of coins
        /// </summary>
        /// <param name="changeAmount"></param>
        /// <returns></returns>
        private IList<int> CreateChange(int changeAmount)
        {
            IList<int> change = new List<int>();

            if (changeAmount < 1)
            {
                return change;
            }

            var allowedCoins = new[] { 100, 50, 20, 10, 5 };
            foreach (int allowedCoin in allowedCoins)
            {
                while (changeAmount - allowedCoin >= 0)
                {
                    change.Add(allowedCoin);
                    changeAmount -= allowedCoin;
                }
            }

            return change;

        }
    }
}