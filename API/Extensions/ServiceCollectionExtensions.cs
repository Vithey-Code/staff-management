using System.Reflection;
using API.Contexts;
using API.Interfaces;
using API.Services;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string CorsPolicy = nameof(CorsPolicy);

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(
                    connectionString: configuration.GetConnectionString("VConnection"),
                    serverVersion: ServerVersion.AutoDetect(configuration.GetConnectionString("VConnection")),
                    optionsBuilder =>
                    {
                        optionsBuilder.EnableRetryOnFailure();
                    });
            });

            return services;
        }
        
        
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IStaffService, StaffService>();
            services.AddTransient<IExcelService, ExcelService>();
            services.AddLazyCache();
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
            return services;
        }
        
        public static void AddInfrastructureMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            return services.AddCors(opt =>
                opt.AddPolicy(CorsPolicy, policy =>
                    policy.AllowAnyHeader()
                        .SetIsOriginAllowed(origin => true)
                        .AllowAnyMethod()
                        .AllowCredentials()
                ));
        }

        public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app) =>
            app.UseCors(CorsPolicy);
    }
}