//using Api.Common;
//using Api.Products;
//using AutoMapper;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Threading.Tasks;

//namespace SC.ShipmentDrafts.Api.ShipmentDrafts
//{
//    /// <summary>
//    /// Returns shipment drafts
//    /// </summary>
//    [Route("/[controller]")]
//    [Authorize]
//    [ApiController]
//    public class PrdocutsController : ApiControllerBase
//    {
//        /// <summary>
//        /// Mapper for dto objects
//        /// </summary>
//        public IMapper Mapper { get; protected set; }

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        /// <param name="mapper"></param>
//        /// <param name="logger"></param>
//        public PrdocutsController(IMapper mapper,
//                                        ILogger<PrdocutsController> logger) : base(logger)
//        {
//            if(mapper == null)
//            {
//                throw new ArgumentNullException(nameof(mapper));
//            }
//            Mapper = mapper;

//        }

//        /// <summary>
//        /// Returns product by id
//        /// </summary>
//        /// <param name="id">id of the product</param>
//        /// <returns>Product for coffee machine</returns>
//        /// <response code="200">Returns the newly created product draft</response>
//        /// <response code="204">If no one item for product was found</response> 
//        /// <response code="404">If the id of order is invalid</response>  
//        [HttpGet, Route("{orderId}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [Produces("application/json")]
//        public async Task<ActionResult<ProductDto>> GetProductAsync(int id)
//        {
//            Logger.LogDebug($"Get shipment draft for order {id}.");

//            if (id < 1)
//            {
//                Logger.LogError($"Order can't be less then 1, but was {id}.");
//                return NotFound();
//            }

//            //int companyId = User.Identity.GetClaimValue<int>(ClaimTypes.CompanyId);


//            if (id == null)
//            {
//                Logger.LogError($"Shipment draft was not created for order with id: {id}.");
//                return NoContent();
//            }

//            var result = Mapper.Map<Product, ProductDto>(product);

//            Logger.LogDebug($"Product {id} was successfully returned.");
//            return result;
//        }


//        /// <summary>
//        /// Creates shipment draft according to overrides
//        /// </summary>
//        /// <param name="orderId">Id of the order to create shipment draft with overrides</param>
//        /// <param name="shipmentOverrideDto">Overrides settings</param>
//        /// <returns>Shipment draft with applied overrides</returns>
//        /// <response code="200">Returns the shipment draft with applied overrides</response>
//        /// <response code="204">If no one item for order was found in appropriate state ready for shipment</response>
//        /// <response code="400">If request body is null</response>  
//        /// <response code="404">If the id of order is negative</response>  
//        [HttpPost, Route("{orderId}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [Produces("application/json")]
//        public async Task<ActionResult<ShipmentDraftDto>> CreateDraftAsync(int orderId, [FromBody] ShipmentOverrideDto shipmentOverrideDto)
//        {
//            Logger.LogDebug($"Create shipment draft for order {orderId} with overrides.");

//            if (orderId < 1)
//            {
//                string error = $"Order can't be less then 1, but was {orderId}.";
//                Logger.LogError(error);
//                return NotFound();
//            }

//            if (shipmentOverrideDto == null)
//            {
//                string errorMsg = "Overrides were not provided.";
//                Logger.LogError("Overrides were not provided.");
//                return BadRequest(errorMsg);
//            }

//            int companyId = User.Identity.GetClaimValue<int>(ClaimTypes.CompanyId);
//            var shipmentOverride = Mapper.Map<ShipmentOverrideDto, ShipmentOverride>(shipmentOverrideDto);

//            ShipmentDraft shipmentDraft = await ShipmentDraftFactory.CreateShipmentDraftAsync(companyId, orderId, shipmentOverride);

//            if (shipmentDraft == null)
//            {
//                Logger.LogError($"Shipment draft was not created for order with id: {orderId}.");
//                return NoContent();
//            }

//            var result = Mapper.Map<ShipmentDraft, ShipmentDraftDto>(shipmentDraft);

//            Logger.LogDebug($"Shipment draft for order {orderId} was successfully created.");
//            return result;
//        }
//    }
//}
