using Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Cloudinary
{
    public static class CloudinaryExtensions
    {
        public static IServiceCollection AddCloudinaryServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPictureService, PictureService>();
            services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
            
            return services;
        }
    }
}