using System;
using System.Threading.Tasks;
using Api.Common;
using Api.Products.Models;
using AutoMapper;
using Domain.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Products
{
    /// <summary>
    /// Returns shipment drafts
    /// </summary>
    [Route("/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductsController : ApiControllerBase
    {
        /// <summary>
        /// Products repository
        /// </summary>
        public IProductsRepository ProductsRepository { get; }

        /// <summary>
        /// Mapper for dto objects
        /// </summary>
        public IMapper Mapper { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="productsRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public ProductsController(IProductsRepository productsRepository,
            IMapper mapper,
                                        ILogger<ProductsController> logger) : base(logger)
        {
            ProductsRepository = productsRepository ?? throw new ArgumentNullException(nameof(productsRepository));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }

        /// <summary>
        /// Returns product by id
        /// </summary>
        /// <param name="id">id of the product</param>
        /// <returns>Product for coffee machine</returns>
        /// <response code="200">Returns the existing product </response>
        /// <response code="204">If product was not found</response> 
        [HttpGet, Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/json")]
        public async Task<ActionResult<ProductDto>> GetProductAsync(int id)
        {
            Logger.LogDebug($"Get product by {id}.");

            if (id < 1)
            {
                Logger.LogError($"Product id can't be less then 1, but was {id}.");
                return NotFound();
            }

            //int companyId = User.Identity.GetClaimValue<int>(ClaimTypes.CompanyId);
            var product = await ProductsRepository.GetAsync(id);
            
            if (product == null)
            {
                Logger.LogError($"Product with {id} doesn't exist.");
                return NoContent();
            }

            ProductDto result = Mapper.Map<Product, ProductDto>(product);

            Logger.LogDebug($"Product {id} was successfully returned.");
            return result;
        }


        /// <summary>
        /// Creates product
        /// </summary>
        /// <param name="model">Overrides settings</param>
        /// <returns></returns>
        /// <response code="201">If product was created</response>
        /// <response code="400">If request body is null</response>   
        [HttpPost, Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> CreateDraftAsync([FromBody] ProductDto model)
        {
            Logger.LogDebug($"Create Product {model?.Name} with overrides.");
            
            if (model == null)
            {
                string errorMsg = "Product entity was not provided.";
                Logger.LogError(errorMsg);
                return BadRequest(errorMsg);
            }
            
            Product product = Mapper.Map<ProductDto, Product>(model);
            
            await ProductsRepository.AddAsync(product);

            Logger.LogDebug($"Product {product.Name} was successfully created.");
            return Ok();
        }
    }
}
