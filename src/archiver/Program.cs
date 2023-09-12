namespace archiver
{
    using archiver.Application.Handlers.ConfigCreator;
    using archiver.Application.Handlers.ConfigLoader;
    using archiver.Application.Handlers.FileArchiver;
    using archiver.Application.Handlers.FolderScanner;
    using archiver.Application.Interfaces;
    using archiver.Core;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public class Program
    {

        public static IMediator _mediator;
        public static IConsoleService _consoleService;

        public static List<string> _lockedFiles;

        public Program(
            IMediator mediator,
            IConsoleService consoleService
        )
        {
            _consoleService = consoleService;
            _mediator = mediator;
            _lockedFiles = new List<string>();

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

            _lockedFiles = new List<string>();
        }


        static async Task Main(string[] args)
        {

            Initialize();
                
            Console.Clear();

            ProgramConfig.ConsoleWidth = Console.WindowWidth;
            ProgramConfig.ResponsiveSpacer = new String('*', ProgramConfig.ConsoleWidth);

            await _consoleService.WriteToConsole(ProgramConfig.ResponsiveSpacer);

            var configCreation = await _mediator.Send(new ConfigCreatorCommand());

            if (configCreation.ConfigCreated == ConfigCreated.False)
            {
                var configResult = await _mediator.Send(new ConfigLoaderCommand());

                if (configResult.ConfigLoaded)
                {
                    var result = await _mediator.Send(new FolderScannerCommand());

                    await ProcessFileList(result.FileList);

                    if (_lockedFiles.Any())
                    {
                        foreach (var file in _lockedFiles)
                        {
                            var processFile = await _mediator.Send(new FileArchiverCommand() { FileName = file });
                        }
                    }
                }

                else
                {
                    await _consoleService.WriteToConsole("There was an issue loading configuration settings... Check config.ini");
                }
            }

            else if (configCreation.ConfigCreated == ConfigCreated.True)
            {
                await _consoleService.WriteToConsole("Config created but requires user setup");
                // user needs to setup their config file after creation
            }

            await _consoleService.WriteToConsole("\r\n" + ProgramConfig.ResponsiveSpacer);
        }

        private static async Task ProcessFileList(IEnumerable<string> fileList, bool retryMode = false)
        {
            var i = 0;

            await _consoleService.WriteToConsole($"{SharedContent.ReturnFormattedDateTimeToString()} {SharedContent.ReturnMessageForHandler(HandlerNames.FileArchiverCommand.ToString())} \r\n");

            foreach (var file in fileList)
            {
                var processFile = await _mediator.Send(new FileArchiverCommand() { FileName = file, FileNumber = i+1, TotalFiles = fileList.Count() } ) ;

                if (!processFile.ArchiveSuccess && !retryMode)
                {
                    _lockedFiles.Add(file);

                } else if (!processFile.ArchiveSuccess && retryMode)
                {
                    await _consoleService.WriteToConsole($"Archiving file: {file} failed after retrying");
                }

                // remove from final locked list if retry mode and file archive was successful
                if (retryMode && processFile.ArchiveSuccess)
                {
                    _lockedFiles.Remove(file);
                }

                i++;
            }

        }
    }

}

