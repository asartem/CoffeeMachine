using System;
using System.Collections.Generic;
using Cm.Domain.Products;

namespace Cm.Domain.Purchases.Models
{
    /// <summary>
    /// Order entity
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Products and purchased qty
        /// </summary>
        public IDictionary<Product, int> ProductsAndQty { get; set; } = new Dictionary<Product, int>();

        /// <summary>
        /// Total cost amount
        /// </summary>
        public int TotalCost
        {
            get
            {
                int totalAmount = 0;
                foreach (KeyValuePair<Product, int> keyValuePair in ProductsAndQty)
                {
                    Product product = keyValuePair.Key;
                    int qty = keyValuePair.Value;

                    totalAmount += product.Price * qty;
                }

                return totalAmount;

            }
        }

        /// <summary>
        /// Change left
        /// </summary>
        public int ChangeAmount { get; set; }

        /// <summary>
        /// Create the instance of the class
        /// </summary>
        /// <param name="productsAndQty"></param>
        public Order(IDictionary<Product, int> productsAndQty)
        {
            ProductsAndQty = productsAndQty ?? throw new ArgumentNullException(nameof(productsAndQty));
        }

        /// <summary>
        /// Set change amount
        /// </summary>
        /// <param name="amount"></param>
        public void SetChange(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }
            
            ChangeAmount = amount;
        }
    }
}