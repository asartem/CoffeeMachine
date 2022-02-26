using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using Api.Application.ApiVersion;
using Api.Common.ExceptionHandling;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Api
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

            services.AddAutoMapper(typeof(Startup).Assembly);

            AddJwtTokenAuthentication(services);

            //services.RegisterShipmentDraftsServices((scopedProvider) =>
            //{
            //    var contextAccessor = scopedProvider.GetRequiredService<IHttpContextAccessor>();
            //    var companyId = contextAccessor.HttpContext.User.Identity.GetClaimValue<int>(ClaimTypes.CompanyId);
            //    return companyId;
            //});
        }


        /// <summary>
        ///     This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();


            //
            // WEB API Components
            // See also: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1#middleware-order
            //
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
            //var jwtConf = new { Secret = "" };//Configuration.GetSection("SC:Auth:Jwt").Get<JwtOptions>(config => config.BindNonPublicProperties = true);
            //var key = System.Text.Encoding.ASCII.GetBytes(jwtConf.Secret);
            //services
            //    .AddAuthentication(x =>
            //    {
            //        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddJwtBearer(x =>
            //    {
            //        // HACK: Custom token parser to translate SC.Web.Auth token format into proper ClaimsIdentity-based SecurityTokenDescriptor
            //        //x.RequireHttpsMetadata = false;
            //        //x.SaveToken = true;
            //        //x.TokenValidationParameters = new TokenValidationParameters
            //        //{
            //        //    ValidateIssuerSigningKey = true,
            //        //    IssuerSigningKey = new SymmetricSecurityKey(key),
            //        //    ValidateIssuer = false,
            //        //    ValidateAudience = false
            //        //};
            //    });

        }

    }
}
