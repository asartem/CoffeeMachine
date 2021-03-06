using Cm.Domain.Common.Dal;
using Cm.Domain.Deposits.Services;
using Cm.Domain.Deposits.Specifications;
using Cm.Domain.Products;
using Cm.Domain.Purchases;
using Cm.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cm.Domain
{
    /// <summary>
    /// Register services
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Register service for this library
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void RegisterDomainServices(this IServiceCollection services, IConfiguration configuration)
        {

            const string connectionStringKey = "DefaultConnection";
            var connectionString = configuration.GetConnectionString(connectionStringKey);
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    b =>
                        b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IPurchaseService, PurchaseService>();

            services.AddSingleton<IValidCoinsSpecification, ValidCoinsSpecification>();
            services.AddSingleton<IUserDepositService, UserDepositService>();
            
        }
    }
}
