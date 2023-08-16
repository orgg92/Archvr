namespace Ryker
{
    using Application.Common;
    using Application.Interfaces;
    using Application.Services;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Runtime.CompilerServices;

    public class Startup
    {
        IConfigurationRoot Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder();
            //.AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddLogging();
            services
                .AddSingleton<IConfigurationRoot>(Configuration)
                .AddMediatR(AppDomain.CurrentDomain.GetAssemblies())
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(BaseHandlerPipelineBehaviour<,>))
                .AddTransient<ILoggerService, LoggerService>()
                .AddTransient<IConsoleService, ConsoleService>();

        }
    }
}