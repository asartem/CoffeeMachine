using Api.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Application.ApiVersion
{
    /// <summary>
    ///     Service version controller
    /// </summary>
    [Route("/app/[controller]")]
    [ApiController]
    public sealed class VersionController : ApiControllerBase
    {
        /// <summary>
        ///     Constructs new <see cref="VersionController" /> instance.
        /// </summary>
        /// <param name="logger">(Optional) Logging service provider.</param>
        public VersionController(ILogger<VersionController> logger = null) : base(logger)
        {
        }


        /// <summary>
        ///     Reports current application version.
        /// </summary>
        /// <returns>Application version details.</returns>
        [HttpGet]
        public ActionResult<ApiVersionModel> Version()
        {
            Logger.Log(LogLevel.Debug, "Return version of the api");

            var apiVersion = new ApiVersionModel();
            return Json(apiVersion);
        }
    }
}