using System;
using System.Threading.Tasks;
using Cm.Api.Api.Authentication.Models;
using Cm.Api.Api.Deposit.Models;
using Cm.Api.Common;
using Cm.Api.Common.CustomExceptions;
using Cm.Domain.Deposits.Services;
using Cm.Domain.Users;
using Cm.Domain.Users.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cm.Api.Api.Deposit
{
    /// <summary>
    /// Returns deposit for user
    /// </summary>
    [Authorize]
    [Route("/[controller]")]
    [ApiController]
    public class DepositsController : ApiControllerBase
    {
        /// <summary>
        /// Service to manage users deposit
        /// </summary>
        public IUserDepositService DepositService { get; }

        /// <summary>
        /// Users repository
        /// </summary>
        public IUsersRepository UsersRepository { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="depositService"></param>
        /// <param name="usersRepository"></param>
        /// <param name="logger"></param>
        public DepositsController(IUserDepositService depositService,
                                  IUsersRepository usersRepository,
                                  ILogger<DepositsController> logger) : base(logger)
        {
            DepositService = depositService ?? throw new ArgumentNullException(nameof(depositService));
            UsersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));

        }

        /// <summary>
        /// Returns current user's deposit
        /// </summary>
        /// <returns>Users deposit</returns>
        /// <response code="200">Returns amount of money </response>
        /// <response code="204">If user was not found</response> 
        [HttpGet, Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/json")]
        public async Task<ActionResult<int>> GetDepositAsync()
        {
            int id = HttpContext.User.Identity.Id();

            Logger.LogDebug($"Get current user's {id} deposit by token.");

            User existingUser = await UsersRepository.GetAsync(id);

            if (existingUser == null)
            {
                string errorMsg = $"User with {id} doesn't exist.";
                Logger.LogError(errorMsg);
                throw new EntityNotFoundException(errorMsg);
            }

            Logger.LogDebug($"User {id} deposit was successfully returned.");
            return existingUser.Deposit;
        }


        /// <summary>
        /// Updates user's deposit
        /// </summary>
        /// <param name="model">User deposit model</param>
        /// <returns></returns>
        /// <response code="200">If user's deposit was updated</response>
        /// <response code="400">If request body is null or invalid</response>   
        [HttpPut, Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<int>> AddToDepositAsync([FromBody] UpdateDepositDto model)
        {
            int id = HttpContext.User.Identity.Id();

            Logger.LogDebug($"Update User's {id} deposit.");

            if (model == null)
            {
                string errorMsg = "User entity was not provided.";
                Logger.LogError(errorMsg);
                return BadRequest(errorMsg);
            }

            if (ModelState.IsValid == false)
            {
                throw new ModelValidationException(ModelState.Values);
            }

            User existingUser = await UsersRepository.GetAsync(id);

            if (existingUser == null)
            {
                string errorMsg = $"User with {id} doesn't exist.";
                Logger.LogError(errorMsg);
                throw new EntityNotFoundException(errorMsg);
            }

            User updatedUser = DepositService.UpdateDeposit(existingUser, model.Deposit);
            await UsersRepository.AddAsync(updatedUser);

            Logger.LogDebug($"User {updatedUser.Id} deposit was successfully update.");
            return updatedUser.Deposit;
        }


        /// <summary>
        /// Reset deposit to 0
        /// </summary>
        /// <returns></returns>
        /// <response code="200">If user's deposit was reset to 0</response>
        /// <response code="404">If user was not found</response>   
        [HttpPut, Route("reset")]
        [Authorize(Roles = UserRoles.Buyer)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> ResetDeposit()
        {
            Logger.LogDebug("Rest user's deposit");

            int id = HttpContext.User.Identity.Id();

            User existingUser = await UsersRepository.GetAsync(id);
            if (existingUser == null)
            {
                string errorMsg = $"User with {id} doesn't exist.";
                Logger.LogError(errorMsg);
                throw new EntityNotFoundException(errorMsg);
            }

            User updatedUser = DepositService.ResetDeposit(existingUser);
            await UsersRepository.AddAsync(updatedUser);

            Logger.LogDebug($"User {id} deposit was successfully removed.");
            return Ok();
        }

        /// <summary>
        /// Disposes all contexts
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            UsersRepository.Dispose();
            base.Dispose(disposing);
        }

    }
}
