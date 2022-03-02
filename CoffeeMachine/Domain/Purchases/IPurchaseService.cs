using System.Collections.Generic;
using System.Threading.Tasks;
using Cm.Domain.Purchases.Models;

namespace Cm.Domain.Purchases
{
    /// <summary>
    /// Interface for service which makes a purchase
    /// </summary>
    public interface IPurchaseService
    {
        Task<Order> CreateOrderAsync(IDictionary<int, int> productIdsAndQty, int buyerId);
    }
}