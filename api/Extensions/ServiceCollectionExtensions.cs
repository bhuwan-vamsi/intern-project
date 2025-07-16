using APIPractice.Repositories;
using APIPractice.Services;
using Microsoft.Extensions.DependencyInjection;

namespace APIPractice.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            // Register Repositories
            services.AddScoped<IItemRepository, ItemRepository>();

            // Register Services
            services.AddScoped<ItemService>();

            return services;
        }
    }
}
