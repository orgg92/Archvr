namespace Rkyver
{
    using Application;
    using Application.Common;
    using Application.Handlers.ConfigCreator;
    using Application.Handlers.ConfigLoader;
    using Application.Handlers.FileArchiver;
    using Application.Handlers.FolderScanner;
    using Application.Services;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Reflection;

    public class Program
    {

        public static IMediator _mediator;
        public static List<string> _lockedFiles;

        public Program(IMediator mediator)
        {
            _mediator = mediator;
            _lockedFiles = new List<string>();
        }


        static async Task Main(string[] args)
        {
            var _serviceCollection = new ServiceCollection()
                 .AddMediatR(AppDomain.CurrentDomain.GetAssemblies())
                 .AddTransient(typeof(IPipelineBehavior<,>), typeof(BaseHandlerPipelineBehaviour<,>))
                 .AddTransient<IMediatorService, MediatorService>()
                 .BuildServiceProvider();

            _mediator = _serviceCollection.GetService<IMediator>();

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
                    Console.WriteLine("There was an issue loading configuration settings... Check config.ini");
                }
            }

            else if (configCreation.ConfigCreated == ConfigCreated.True)
            {

                Console.WriteLine("Config created but requires user setup");
                // user needs to setup their config file after creation


            }
        }

        private static async Task ProcessFileList(IEnumerable<string> fileList, bool retryMode = false)
        {
            foreach (var file in fileList)
            {
                var processFile = await _mediator.Send(new FileArchiverCommand() { FileName = file });

                if (!processFile.ArchiveSuccess && !retryMode)
                {
                    // add to locked list
                    _lockedFiles.Add(file);
                } else if (!processFile.ArchiveSuccess && retryMode)
                {
                    Console.WriteLine($"Archiving file: {file} failed after retrying");
                }

              
            }

        }
    }

}

