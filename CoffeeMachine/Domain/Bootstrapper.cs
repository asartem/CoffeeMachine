using Domain.Common.Dal;
using Domain.Products;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class Bootstrapper
    {

        public static void RegisterDalServices(this IServiceCollection services, IConfiguration configuration)
        {

            const string connectionStringKey = "DefaultConnection";
            var connectionString = configuration.GetConnectionString(connectionStringKey);
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    b =>
                        b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
            
            //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            
        }
    }
}
