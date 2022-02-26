using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Api.Common
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
        /// Logger for all controllers
        /// </summary>
        public ILogger<ApiControllerBase> Logger { get; protected set; }
    }
}