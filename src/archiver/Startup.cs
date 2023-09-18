namespace archiver
{
    using archiver.Application.Common;
    using archiver.Application.Interfaces;
    using archiver.Application.Services;
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
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(BaseHandlerPipelineBehaviour<,>))
                .AddTransient<IArchiver, Archiver>()
                .AddTransient<IConfigService, ConfigService>()
                .AddTransient<ILoggerService, LoggerService>()
                .AddTransient<IIOService, IOService>()
                .AddTransient<IConsoleService, ConsoleService>()
            ;
        }

        public void RegisterPipelines(IServiceCollection services)
        {
            services
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(BaseHandlerPipelineBehaviour<,>));
        }

    }
}