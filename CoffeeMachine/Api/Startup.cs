using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using Cm.Api.Api.Authentication.Services;
using Cm.Api.Application.ApiVersion;
using Cm.Api.Application.Settings;
using Cm.Api.Common.ExceptionHandling;
using Cm.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Cm.Api
{

    /// <summary>
    ///     Application startup procedures container.
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///     construct new <see cref="Startup" /> instance.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        ///     Application configuration.
        /// </summary>
        public IConfiguration Configuration { get; }


        /// <summary>
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Services provider.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCors()
                .AddRouting(options => { options.LowercaseUrls = true; })
                .AddSwaggerGen(options =>
                {
                    var version = new ApiVersionModel();
                    options.SwaggerDoc("v1", new OpenApiInfo()
                    {
                        Version = version.Product.ToString(),
                        Title = @"Coffee Machine service",
                        Description = "Micro service to emulate coffee machine"

                    });

                    string fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    string renderedXmlWithCommentsFilePath = Path.Combine(AppContext.BaseDirectory, fileName);
                    options.IncludeXmlComments(renderedXmlWithCommentsFilePath);
                })
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                });

            services.RegisterDomainServices(Configuration);

            AddJwtTokenAuthentication(services);
        }


        /// <summary>
        ///     This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            
            app
                .UseRouting()
                .UseCors()
                .UseAuthentication()
                .UseAuthorization()
                .UseMiddleware<ExceptionHandlingMiddleware>()
                .UseEndpoints(endpoints => endpoints
                    .MapControllers()
                )
                .UseRewriter(new RewriteOptions()
                    .AddRedirect(@"^$", "/app/version", (int)HttpStatusCode.MovedPermanently)
                    .AddRedirect(@"^swagger$", "/docs", (int)HttpStatusCode.MovedPermanently)
                )
                .UseSwagger(options =>
                {
                    options.RouteTemplate = "docs/swagger/{documentname}/swagger.json";
                })
                .UseSwaggerUI(x =>
                {
                    x.SwaggerEndpoint("swagger/v1/swagger.json", "ApiSwagger");
                    x.RoutePrefix = "docs";

                });
        }


        /// <summary>
        /// Adds standard .net token authentication
        /// </summary>
        /// <param name="services"></param>
        private void AddJwtTokenAuthentication(IServiceCollection services)
        {
            IConfigurationSection appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddScoped<IAuthenticateService, AuthenticateService>();
        }

    }
}
