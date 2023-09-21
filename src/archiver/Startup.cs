namespace archiver
{
    using archiver.Application.Common;
    using archiver.Core.Interfaces;
    using archiver.Core.Services;
    using archiver.Infrastructure.Interfaces;
    using archiver.Infrastructure.Services;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<IConfiguration>(Configuration)
                .AddMediatR(AppDomain.CurrentDomain.GetAssemblies())
                .AddSingleton(typeof(IPipelineBehavior<,>), typeof(BaseHandlerPipelineBehaviour<,>))
                .AddSingleton<IConfigService, ConfigService>()
                .AddSingleton<ILoggerService, LoggerService>()
                .AddSingleton<IIOService, IOService>()
                .AddSingleton<IConsoleService, ConsoleService>()
            ;
        }

        public void RegisterPipelines(IServiceCollection services)
        {
            services
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(BaseHandlerPipelineBehaviour<,>));
        }

    }
}