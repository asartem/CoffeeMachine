using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using Cm.Api.Api.Authentication.Models;
using Cm.Api.Api.Authentication.Services;
using Cm.Api.Common;
using Cm.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cm.Api.Api.Authentication
{
    /// <summary>
    /// Authentication controller
    /// </summary>
    [Route("/[controller]")]
    [ApiController]
    public class LoginController : ApiControllerBase
    {
        /// <summary>
        /// Users repository
        /// </summary>
        public IUsersRepository UsersRepository { get; }

        /// <summary>
        /// Services which authenticates users
        /// </summary>
        public IAuthenticateService AuthenticateService { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="usersRepository"></param>
        /// <param name="authenticateService"></param>
        /// <param name="logger"></param>
        public LoginController(IUsersRepository usersRepository,
                                IAuthenticateService authenticateService,
                                  ILogger<LoginController> logger) : base(logger)
        {
            UsersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            AuthenticateService = authenticateService;
        }

        /// <summary>
        /// Creates buyer
        /// </summary>
        /// <param name="model">User</param>
        /// <returns></returns>
        /// <response code="201">If buyer was created</response>
        /// <response code="400">If request body is null or invalid</response>   
        [HttpPost, Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> CreateToken([FromBody] UserAuthenticationDto model)
        {
            Logger.LogDebug($"Authenticate user {model?.UserName}.");

            if (model == null)
            {
                string errorMsg = "User entity was not provided.";
                Logger.LogError(errorMsg);
                throw new ArgumentNullException(errorMsg);
            }

            string resultToken = await AuthenticateService.GetToken(model.UserName, model.Password);

            if (string.IsNullOrWhiteSpace(resultToken))
            {
                throw new AuthenticationException();
            }

            Logger.LogDebug($"Token for user {model.UserName} was successfully created.");
            return Ok(resultToken);
        }

    }
}
