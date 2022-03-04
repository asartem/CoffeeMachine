using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cm.Domain.Purchases.Models;

namespace Cm.Domain.Purchases
{
    /// <summary>
    /// Interface for service which makes a purchase
    /// </summary>
    public interface IPurchaseService : IDisposable
    {
        /// <summary>
        /// Make a purchase
        /// </summary>
        /// <returns></returns>
        Task<Order> CreateOrderAsync(IDictionary<int, int> productIdsAndQty, int buyerId);
    }
}