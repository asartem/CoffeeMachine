using System;
using System.IO;
using Cm.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;

namespace Cm.Tests
{
    [SetUpFixture]
    public class AllTestsSetup
    {
        protected virtual string ConfigFileName => "appsettings.json";

        protected string UnitTestsBinFolder = Directory.GetCurrentDirectory();

        public static ServiceCollection ServiceCollection;

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
            ServiceCollection = GetServiceCollectionWithCommonSettings();

            try
            {
                var serviceProvider = ServiceCollection.BuildServiceProvider();
                var configuration = serviceProvider.GetService<IConfiguration>();
                ServiceCollection.RegisterDalServices(configuration);
                serviceProvider.Dispose();
                serviceProvider = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
            //SetupPreDefinedTestData();

        }

        private static void SetupPreDefinedTestData()
        {
            var serviceProvider = ServiceCollection.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();
            // var connectionString = configuration[Domain.TestData.PathToContentDbSetting];

            //var connectionStringTestDataFactory = serviceProvider.GetService<ConnectionStringTestDataFactory>();
            
            serviceProvider.Dispose();
        }

        private ServiceCollection GetServiceCollectionWithCommonSettings()
        {
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var serviceCollection = new ServiceCollection();
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(UnitTestsBinFolder)
                .AddJsonFile(ConfigFileName, optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json",
                    optional: true, reloadOnChange: true);


            IConfiguration hostConfiguration = configurationBuilder.Build();
            serviceCollection.TryAddSingleton(hostConfiguration);

            return serviceCollection;
        }

    }
}