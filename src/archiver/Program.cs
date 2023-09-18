namespace archiver
{
    using archiver.Application.Handlers.ConfigCreator;
    using archiver.Application.Handlers.ConfigLoader;
    using archiver.Application.Handlers.FileArchiver;
    using archiver.Application.Handlers.FolderScanner;
    using archiver.Core;
    using archiver.Core.Enum;
    using archiver.Infrastructure.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public class Program 
    {

        public static IMediator _mediator;
        public static IConsoleService _consoleService;
        public static IArchiver _archiver;

        public Program(
            IMediator mediator,
            IConsoleService consoleService,
            IArchiver archiver
        )
        {
            _consoleService = consoleService;
            _mediator = mediator;
            _archiver = archiver;

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

            _archiver = serviceProvider
                .GetService<IArchiver>();

        }


        static async Task Main(string[] args)
        {
            //var archiver = new Archiver(_mediator, _consoleService);
            //var archiver = new Archiver(_mediator, _consoleService);

            Initialize();
            await _archiver.Initialize();


        }
    }

}

