using System;
using System.Data;
using System.Net;
using System.Net.Mime;
using System.Security.Authentication;
using System.Threading.Tasks;
using Cm.Api.Common.CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cm.Api.Common.ExceptionHandling
{
    /// <summary>
    /// Middleware to handle exceptions in a single place
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;

        /// <summary>
        /// Create the instance of the class
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        /// <summary>
        /// Next step of the request pipe line
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception e)
            {
                logger.LogError($"An exception has acquired: {e}");
                await HandleExceptionAsync(httpContext, e);
            }
        }

        /// <summary>
        /// Handle response with exceptions
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode responseStatusCode = HttpStatusCode.InternalServerError;

            if (exception is DataException)
            {
                if (exception.InnerException is ApplicationException)
                {
                    exception = exception.InnerException;
                }
            }

            string errorMessage = exception.Message;

            
            if (exception is ApplicationException)
            {
                responseStatusCode = HttpStatusCode.UnprocessableEntity;
                errorMessage = $"Invalid entity. {errorMessage}";
            }

            if (exception is EntityNotFoundException)
            {
                responseStatusCode = HttpStatusCode.NotFound;
                errorMessage = exception.Message;
            }

            if (exception is AuthenticationException)
            {
                responseStatusCode = HttpStatusCode.Unauthorized;
                errorMessage = "User name or password are invalid";
            }

            if (string.IsNullOrWhiteSpace(context.Response.ContentType))
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;
            }

            context.Response.StatusCode = (int)responseStatusCode;
            var errorDetailsAsString = new ErrorDetails(errorMessage).ToString();

            return context.Response.WriteAsync(errorDetailsAsString);
        }
    }
}
