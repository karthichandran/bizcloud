using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Services
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
          services.AddCors(options =>
          {
              //options.AddPolicy("CorsPolicy", builder =>
              //    builder.AllowAnyOrigin()
              //    .AllowAnyMethod()
              //    .AllowAnyHeader());

              options.AddPolicy("default", policy =>
              {
                  policy.AllowAnyOrigin()
                      
                      .AllowAnyHeader()
                      .AllowAnyMethod();
              });

          });

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {

            });

    }
}
