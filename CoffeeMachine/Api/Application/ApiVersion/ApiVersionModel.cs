using System.Diagnostics;

namespace Api.Application.ApiVersion
{
    /// <summary>
    /// Model of the api service version
    /// </summary>
    public class ApiVersionModel
    {
        /// <summary>
        /// Assembly
        /// </summary>
        public string Assembly { get; protected set; }

        /// <summary>
        /// File version
        /// </summary>
        public string File { get; protected set; }

        /// <summary>
        /// Product version
        /// </summary>
        public string Product { get; protected set; }

        /// <summary>
        /// Create the instance of the class
        /// </summary>
        public ApiVersionModel()
        {
            Assembly = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            File = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location)
                .FileVersion;
            Product = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location)
                .ProductVersion;
        }
    }
}