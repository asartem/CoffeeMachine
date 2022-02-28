using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cm.Api.Api.Authentication;
using Cm.Api.Api.Authentication.Models;
using Cm.Api.Api.Products.Models;
using Cm.Api.Common;
using Cm.Api.Common.CustomExceptions;
using Cm.Domain.Products;
using Cm.Domain.Users;
using Cm.Domain.Users.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cm.Api.Api.Products
{
    /// <summary>
    /// Returns products
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
        /// Users repository
        /// </summary>
        public IUsersRepository UsersRepository { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="productsRepository"></param>
        /// <param name="usersRepository"></param>
        /// <param name="logger"></param>
        public ProductsController(IProductsRepository productsRepository,
                                    IUsersRepository usersRepository,
                                    ILogger<ProductsController> logger) : base(logger)
        {
            ProductsRepository = productsRepository ?? throw new ArgumentNullException(nameof(productsRepository));
            UsersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));

        }

        /// <summary>
        /// Returns product by id
        /// </summary>
        /// <param name="id">id of the product</param>
        /// <returns>Product for coffee machine</returns>
        /// <response code="200">Returns the existing product </response>
        /// <response code="204">If product was not found</response> 
        [HttpGet, Route("{id}")]
        [AllowAnonymous]
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

            Product product = await ProductsRepository.GetAsync(id);

            if (product == null)
            {
                Logger.LogError($"Product with {id} doesn't exist.");
                return NoContent();
            }

            var result = new ProductDto(product);

            Logger.LogDebug($"Product {id} was successfully returned.");
            return result;
        }

        /// <summary>
        /// Returns all products
        /// </summary>
        /// <returns>Product for coffee machine</returns>
        /// <response code="200">Returns the existing product </response>
        /// <response code="204">If product was not found</response> 
        [HttpGet, Route("")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/json")]
        public async Task<ActionResult<ICollection<ProductDto>>> GetAllProductAsync()
        {
            Logger.LogDebug("Get all products.");

            List<Product> products = (await ProductsRepository.GetAllAsync()).ToList();

            if (products.Any() == false)
            {
                Logger.LogError("There are no products");
                return NoContent();
            }

            IEnumerable<ProductDto> result = ProductDto.ToDto(products);

            Logger.LogDebug("All products were successfully returned.");
            return Ok(result);
        }


        /// <summary>
        /// Creates product
        /// </summary>
        /// <param name="model">Product</param>
        /// <returns></returns>
        /// <response code="201">If product was created</response>
        /// <response code="400">If request body is null or invalid</response>   
        [HttpPost, Route("")]
        [Authorize(Roles = UserRoles.Seller)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> CreateProductAsync([FromBody] CreateProductDto model)
        {
            Logger.LogDebug($"Create Product {model?.Name}.");

            if (model == null)
            {
                string errorMsg = "Product entity was not provided.";
                Logger.LogError(errorMsg);
                return BadRequest(errorMsg);
            }

            int userId = User.Identity.Id();
            User user = await UsersRepository.GetAsync(userId);
            Product product = model.ToEntity(user);

            await ProductsRepository.AddAsync(product);

            Logger.LogDebug($"Product {product.Name} was successfully created.");
            return Ok(product);
        }


        /// <summary>
        /// Updates product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model">Product model</param>
        /// <returns></returns>
        /// <response code="201">If product was created</response>
        /// <response code="400">If request body is null or invalid</response>   
        [HttpPut, Route("{id}")]
        [Authorize(Roles = UserRoles.Seller)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> UpdateProductAsync(int id, [FromBody] UpdateProductDto model)
        {
            Logger.LogDebug($"Update Product {model?.Name}.");

            if (model == null)
            {
                string errorMsg = "Product entity was not provided.";
                Logger.LogError(errorMsg);
                return BadRequest(errorMsg);
            }

            Product existingProduct = await ProductsRepository.GetAsync(id);
            if (existingProduct == null)
            {
                throw new EntityNotFoundException();
            }

            Product updatedProduct = model.Update(existingProduct);

            await ProductsRepository.AddAsync(updatedProduct);

            Logger.LogDebug($"Product {updatedProduct.Id} was successfully update.");
            return Ok();
        }


        /// <summary>
        /// Deletes product
        /// </summary>
        /// <param name="id">id of the product</param>
        /// <returns></returns>
        /// <response code="200">If product was deleted</response>
        /// <response code="204">If there is nothing to delete</response>
        /// <response code="400">If request body is null or invalid</response>   
        [HttpDelete, Route("{id}")]
        [Authorize(Roles = UserRoles.Seller)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            Logger.LogDebug($"Delete product with id {id}.");

            Product existingProduct = await ProductsRepository.GetAsync(id);
            if (existingProduct == null)
            {
                return NoContent();
            }

            await ProductsRepository.RemoveAsync(existingProduct);

            Logger.LogDebug($"Product {id} was successfully removed.");
            return Ok();
        }
    }
}
