using Newtonsoft.Json;

namespace Api.Common.ExceptionHandling
{
    /// <summary>
    /// Response for errors
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; protected set; }

        /// <summary>
        /// Create the instance of the class
        /// </summary>
        /// <param name="errorMessage"></param>
        public ErrorDetails(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Converts object to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}