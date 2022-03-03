using System;
using System.IO;
using System.Reflection;
using Cm.Api.Application.Settings;
using Cm.Domain;
using Cm.HostService;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace Cm.Tests
{
    [SetUpFixture]
    public class AllTestsSetup : IDisposable
    {
        protected virtual string ConfigFileName => "appsettings.json";
        public static string JwtSecret { get; private set; }

        protected string UnitTestsBinFolder = Directory.GetCurrentDirectory();

        public static WebApplicationFactory<Program> Factory;
        public static ServiceCollection ServiceCollection;

        string environmentName;

        public AllTestsSetup()
        {
            //NOTE: Env variables don't work for tests. Details: https://thebrokentest.com/managing-config-in-net-core/
#if PROD
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
#else 
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
#endif
        }


        [OneTimeSetUp]
        public void OneTime()
        {

            environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, appBuilder) =>
                {
                    appBuilder.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile(ConfigFileName, optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{environmentName}.json",
                            optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();

                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        appBuilder.AddUserSecrets<Program>();
                    }

                });
            });



            ServiceCollection = GetServiceCollectionWithCommonSettings();


            var serviceProvider = ServiceCollection.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();
            var appSettingsSection = configuration.GetSection("AppSettings");
            ServiceCollection.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            JwtSecret = appSettings.Secret;

            ServiceCollection.RegisterDalServices(configuration);
            serviceProvider.Dispose();

            
        }


        private ServiceCollection GetServiceCollectionWithCommonSettings()
        {
            var serviceCollection = new ServiceCollection();
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(UnitTestsBinFolder)
                .AddJsonFile(ConfigFileName, optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json",
                    optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (environmentName != null && environmentName.Contains("Develop"))
            {
                var appAssembly = Assembly.GetExecutingAssembly();
                configurationBuilder.AddUserSecrets(appAssembly, optional: true);
            }

            IConfiguration hostConfiguration = configurationBuilder.Build();
            serviceCollection.TryAddSingleton(hostConfiguration);

            return serviceCollection;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                Factory?.Dispose();
                Factory = null;
            }

            disposed = true;
        }


    }
}