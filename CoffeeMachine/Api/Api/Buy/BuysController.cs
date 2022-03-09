using System;
using System.Threading.Tasks;
using Cm.Api.Api.Authentication.Models;
using Cm.Api.Api.Buy.Models;
using Cm.Api.Common;
using Cm.Domain.Purchases;
using Cm.Domain.Purchases.Models;
using Cm.Domain.Users.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cm.Api.Api.Buy
{
    /// <summary>
    /// Returns products
    /// </summary>
    [Route("/[controller]")]
    [ApiController]
    public class BuysController : ApiControllerBase
    {
        /// <summary>
        /// Purchase repository
        /// </summary>
        public IPurchaseService PurchaseService { get; }
        

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="purchaseService"></param>
        /// <param name="logger"></param>
        public BuysController(IPurchaseService purchaseService,
                                    ILogger<BuysController> logger) : base(logger)
        {
            PurchaseService = purchaseService ?? throw new ArgumentNullException(nameof(purchaseService));
        }


        /// <summary>
        /// Creates new purchase
        /// </summary>
        /// <param name="model">purchase</param>
        /// <returns></returns>
        /// <response code="201">If purchase was created</response>
        /// <response code="400">If request body is null or invalid</response>    
        [HttpPost, Route("")]
        [Authorize(Roles = UserRoles.Buyer)]
        //[AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<OrderDto>> CreateOrderAsync([FromBody] CreateOrderDto model)
        {
            int buyerId = HttpContext.User.Identity.Id();
            Logger.LogDebug($"Create Order for user {buyerId}.");

            if (model == null)
            {
                string errorMsg = "Order model was not provided.";
                Logger.LogError(errorMsg);
                return BadRequest(errorMsg);
            }

            Order order = await PurchaseService.CreateOrderAsync(model.ProductsAndQuantity, buyerId);

            var result = new OrderDto(order);
            Logger.LogDebug("Purchase was made");
            return Ok(result);
        }

        /// <summary>
        /// Disposes all contexts
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            PurchaseService.Dispose();
            base.Dispose(disposing);
        }

    }
}
