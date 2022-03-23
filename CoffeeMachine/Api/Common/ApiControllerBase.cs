using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Cm.Api.Common
{
    /// <summary>
    ///     Base class for all controllers
    /// </summary>
    public abstract class ApiControllerBase : Controller
    {
        /// <summary>
        /// Create the instance of the class
        /// </summary>
        protected ApiControllerBase(ILogger<ApiControllerBase> logger = null)
        {
            Logger = logger ?? NullLogger<ApiControllerBase>.Instance;
        }

        /// <summary>
        /// Logger fAuthenticateServiceor all controllers
        /// </summary>
        public ILogger<ApiControllerBase> Logger { get; protected set; }
    }
}