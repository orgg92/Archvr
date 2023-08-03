namespace Application
{
    using Application.Handlers.ConfigLoader;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly())
                .AddMediatR(typeof(ConfigLoaderCommand));


            return services;
        }
    }

}