namespace archiver
{
    using archiver.Infrastructure.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public class Program
    {
        public static IMediator _mediator;
        public static IConsoleService _consoleService;

        public Program(
            IMediator mediator,
            IConsoleService consoleService
        )
        {
            _consoleService = consoleService;
            _mediator = mediator;

            Initialize();
        }

        public static void Initialize()
        {
            var config = new ConfigurationBuilder()
                .Build();

            IServiceCollection services = new ServiceCollection();

            Startup startup = new Startup(config);
            startup.ConfigureServices(services);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            _consoleService = serviceProvider
                .GetService<IConsoleService>();

            _mediator = serviceProvider
                .GetService<IMediator>();

        }


        static async Task Main(string[] args)
        {
            var archiver = new Archiver(_mediator, _consoleService);

            Initialize();
            await archiver.Initialize();


        }
    }

}

