namespace archiver
{
    using archiver.Application.Common;
    using archiver.Application.Interfaces;
    using archiver.Application.Services;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Runtime.CompilerServices;

    public class Startup
    {
        IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddLogging();
            services
                .AddSingleton<IConfiguration>(Configuration)
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(BaseHandlerPipelineBehaviour<,>))
                .AddMediatR(AppDomain.CurrentDomain.GetAssemblies())
                .AddTransient<IConfigService, ConfigService>()
                .AddTransient<ILoggerService, LoggerService>()
                .AddTransient<IConsoleService, ConsoleService>();
            ;
        }

        public void RegisterPipelines(IServiceCollection services)
        {
            services
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(BaseHandlerPipelineBehaviour<,>));
        }

    }
}