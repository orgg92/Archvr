namespace Rkyver
{
    using Application;
    using Application.Common;
    using Application.Handlers.ConfigCreator;
    using Application.Handlers.ConfigLoader;
    using Application.Handlers.FileArchiver;
    using Application.Handlers.FolderScanner;
    using Application.Interfaces;
    using Application.Services;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Reflection;

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
        }


        static async Task Main(string[] args)
        {

            Console.Clear();

            _lockedFiles = new List<string>();

            var _serviceCollection = new ServiceCollection()
                 .AddMediatR(AppDomain.CurrentDomain.GetAssemblies())
                 .AddTransient(typeof(IPipelineBehavior<,>), typeof(BaseHandlerPipelineBehaviour<,>))
                 .AddTransient<IMediatorService, MediatorService>()
                 .AddSingleton<IConsoleService, ConsoleService>()
                 .AddSingleton<ILoggerService, LoggerService>()
                 .BuildServiceProvider();

            _mediator = _serviceCollection.GetService<IMediator>();
            _consoleService = _serviceCollection.GetService<IConsoleService>();


            SharedContent.ConsoleWidth = Console.WindowWidth;
            SharedContent.ResponsiveSpacer = new String('*', SharedContent.ConsoleWidth);

            await _consoleService.WriteToConsole(SharedContent.ResponsiveSpacer);

            var configCreation = await _mediator.Send(new ConfigCreatorCommand());

            if (configCreation.ConfigCreated == ConfigCreated.False)
            {
                // load config if creation if config exists 

                var configResult = await _mediator.Send(new ConfigLoaderCommand());

                // scan config for source paths to archive

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

            await _consoleService.WriteToConsole("\r\n" + SharedContent.ResponsiveSpacer);
        }

        private static async Task ProcessFileList(IEnumerable<string> fileList, bool retryMode = false)
        {
            var i = 0;

            await _consoleService.WriteToConsole($"[{DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm")}] {SharedContent.ReturnMessageForHandler(HandlerNames.FileArchiverCommand.ToString())} \r\n");

            foreach (var file in fileList)
            {
                var processFile = await _mediator.Send(new FileArchiverCommand() { FileName = file, FileNumber = i+1, TotalFiles = fileList.Count() } ) ;

                if (!processFile.ArchiveSuccess && !retryMode)
                {
                    // add to locked list
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

