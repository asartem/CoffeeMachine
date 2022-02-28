using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Cm.Api.Api.Authentication;
using Cm.Api.Api.Authentication.Models;
using Cm.Api.Api.Users.Models;
using Cm.Api.Common;
using Cm.Api.Common.CustomExceptions;
using Cm.Domain.Common.Dal;
using Cm.Domain.Users;
using Cm.Domain.Users.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cm.Api.Api.Users
{
    /// <summary>
    /// Returns users
    /// </summary>
    [Authorize]
    [Route("/[controller]")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        /// <summary>
        /// Users repository
        /// </summary>
        public IUsersRepository UsersRepository { get; }

        /// <summary>
        /// Repository for userRoles
        /// </summary>
        public IRepository<UserRole> UserRolesRepository { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="usersRepository"></param>
        /// <param name="userRolesRepository"></param>
        /// <param name="logger"></param>
        public UsersController(IUsersRepository usersRepository,
                                IRepository<UserRole> userRolesRepository,
                                  ILogger<UsersController> logger) : base(logger)
        {
            UsersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            UserRolesRepository = userRolesRepository ?? throw new ArgumentNullException(nameof(userRolesRepository));

        }

        /// <summary>
        /// Returns current user 
        /// </summary>
        /// <returns>User for coffee machine</returns>
        /// <response code="200">Returns the existing user </response>
        /// <response code="204">If user was not found</response> 
        [HttpGet, Route("current")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/json")]
        public async Task<ActionResult<UserDto>> GetUserAsync()
        {
            Logger.LogDebug("Get current user by token.");
            int id = HttpContext.User.Identity.Id();
            User user = await UsersRepository.GetAsync(id);

            if (user == null)
            {
                Logger.LogError($"User with {id} doesn't exist.");
                return NoContent();
            }

            UserDto result = new UserDto(user);

            Logger.LogDebug($"User {id} was successfully returned.");
            return result;
        }


        /// <summary>
        /// Creates buyer
        /// </summary>
        /// <param name="model">User</param>
        /// <returns></returns>
        /// <response code="201">If buyer was created</response>
        /// <response code="400">If request body is null or invalid</response>   
        [HttpPost, Route("buyer")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> CreateBuyerAsync([FromBody] CreateUserDto model)
        {
            Logger.LogDebug($"Create Buyer {model?.UserName}.");

            

            User user = await CreateUserAsync(model, UserRoles.Buyer);
            UserDto userDto = new UserDto(user);

            return Ok(userDto);
        }


        /// <summary>
        /// Creates Seller
        /// </summary>
        /// <param name="model">User</param>
        /// <returns></returns>
        /// <response code="201">If seller was created</response>
        /// <response code="400">If request body is null or invalid</response>
        /// <response code="409">If seller already exist</response>   
        [HttpPost, Route("seller")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Produces("application/json")]
        public async Task<ActionResult> CreateSellerAsync([FromBody] CreateUserDto model)
        {
            Logger.LogDebug($"Create Seller {model?.UserName}.");

            bool isSellerExisting =
                (await UsersRepository.FindAsync(x => x.Role.Name == UserRoles.Seller)).Any();

            if (isSellerExisting)
            {
                return Conflict("Seller already exist. Only one seller is allowed");
            }

            User user = await CreateUserAsync(model, UserRoles.Seller);
            UserDto userDto = new UserDto(user);

            return Ok(userDto);
        }


        /// <summary>
        /// Updates user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model">User model</param>
        /// <returns></returns>
        /// <response code="201">If user was created</response>
        /// <response code="400">If request body is null or invalid</response>   
        [HttpPut, Route("current")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> UpdateUserAsync([FromBody] UpdateUserDto model)
        {
            int id = HttpContext.User.Identity.Id();

            Logger.LogDebug($"Update User {id}.");

            if (model == null)
            {
                string errorMsg = "User entity was not provided.";
                Logger.LogError(errorMsg);
                return BadRequest(errorMsg);
            }

            User existingUser = await UsersRepository.GetAsync(id);
            if (existingUser == null)
            {
                throw new EntityNotFoundException();
            }

            User updatedUser = model.Update(existingUser);

            await UsersRepository.AddAsync(updatedUser);
            var user = new UserDto(updatedUser);

            Logger.LogDebug($"User {updatedUser.Id} was successfully update.");
            return Ok(user);
        }


        /// <summary>
        /// Deletes user
        /// </summary>
        /// <param name="id">id of the user</param>
        /// <returns></returns>
        /// <response code="200">If user was deleted</response>
        /// <response code="204">If there is nothing to delete</response>
        /// <response code="400">If request body is null or invalid</response>   
        [HttpDelete, Route("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteUserAsync(int id)
        {
            Logger.LogDebug($"Delete user with id {id}.");

            User existingUser = await UsersRepository.GetAsync(id);
            if (existingUser == null)
            {
                return NoContent();
            }

            await UsersRepository.RemoveAsync(existingUser);

            Logger.LogDebug($"User {id} was successfully removed.");
            return Ok();
        }


        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="model"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        private async Task<User> CreateUserAsync(CreateUserDto model, string roleName)
        {
            if (model == null)
            {
                string errorMsg = "User entity was not provided.";
                Logger.LogError(errorMsg);
                throw new ArgumentNullException(errorMsg);
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            UserRole role = (await UserRolesRepository.FindAsync(x => x.Name == roleName)).Single();
            User user = model.ToEntity(role);

            await UsersRepository.AddAsync(user);

            Logger.LogDebug($"User {user.Name} was successfully created.");
            return user;
        }
    }
}
