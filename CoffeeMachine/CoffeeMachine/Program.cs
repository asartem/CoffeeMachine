using System;
using System.Linq;
using System.Reflection;
using Cm.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using ILogger = NLog.ILogger;


namespace Cm.HostService
{

    /// <summary>
    /// Main class
    /// </summary>
    public class Program
    {
        private static ILogger log;
        
        /// <summary>
        /// Entry point for service
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {

            log = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                IHostBuilder hostBuilder = CreateHostBuilder(args);

                string serviceName = Assembly.GetExecutingAssembly().FullName;
                log.Info($"Starting service {serviceName}");

                IHost host = hostBuilder.Build();
                host.Run();

                log.Warn("Stop service");
            }
            catch (Exception e)
            {
                log.Error(e, "Api service has been stopped");
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        /// <summary>
        /// Builds host
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            // NOTE: .NET Core 3.x CommandLineConfigurationProvider does not support switches without values
            //       See also: https://github.com/aspnet/Configuration/issues/780
            const string consoleArg = "--console";
            bool isConsoleMode = args.Any(x => x == consoleArg);
            if (isConsoleMode) args = args.Where(a => a != consoleArg).ToArray();

            IHostBuilder builder = Host.CreateDefaultBuilder(args);

            ConfigureHostApplicationComponents(builder);

            if (isConsoleMode)
                ConfigureServiceAsConsoleApplication(builder);
            else
                builder.UseWindowsService();


            return builder;
        }

        /// <summary>
        /// Configure running as service
        /// </summary>
        /// <param name="hostBuilder"></param>
        private static void ConfigureHostApplicationComponents(IHostBuilder hostBuilder)
        {
            hostBuilder
                .ConfigureLogging((hostContext, logging) => logging
                    .ClearProviders()
                    .AddConfiguration(hostContext.Configuration.GetSection("Logging"))
                    .AddNLog()
                )
                .UseNLog()
                .ConfigureServices((hostContext, services) => services
                    .Configure<KestrelServerOptions>(hostContext.Configuration.GetSection("Kestrel"))
                )
                .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder.UseStartup<Startup>()
                );
        }

        /// <summary>
        /// Configure running of service as console
        /// </summary>
        /// <param name="builder"></param>
        private static void ConfigureServiceAsConsoleApplication(IHostBuilder builder)
        {
            log.Info("Use console mode to start application");

            builder.ConfigureServices((hostContext, services) =>
                {
                    services
                        .Configure<ConsoleLifetimeOptions>(hostContext.Configuration.GetSection("Console"));
                })
                .UseConsoleLifetime();
        }
    }
}
