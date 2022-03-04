using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cm.Api.Common.CustomExceptions
{
    /// <summary>
    /// Returns validation exceptions
    /// </summary>
    public class ModelValidationException : ApplicationException
    {
        /// <summary>
        /// Creates instance of the class
        /// </summary>
        /// <param name="modelStateValues"></param>
        public ModelValidationException(ModelStateDictionary.ValueEnumerable modelStateValues) : base(CollectException(modelStateValues))
        {
        }

        /// <summary>
        /// Collects all errors of the model
        /// </summary>
        /// <param name="modelStateValues"></param>
        /// <returns></returns>
        public static string CollectException(ModelStateDictionary.ValueEnumerable modelStateValues)
        {
            IEnumerable<ModelError> allErrors = modelStateValues.SelectMany(x => x.Errors);
            string errorResult = string.Join(", ", allErrors).Trim().TrimEnd(',');
            return errorResult;
        }
    }
}
