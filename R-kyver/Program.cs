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

        static async Task Main(string[] args)
        {
            var _serviceCollection = new ServiceCollection()
                 .AddMediatR(AppDomain.CurrentDomain.GetAssemblies())
                 .AddTransient(typeof(IPipelineBehavior<,>), typeof(ProgramNotificationPipelineBehaviour<,>))
                 .AddTransient<IMediatorService, MediatorService>()
                 .BuildServiceProvider();

            var _mediator = _serviceCollection.GetService<IMediator>();

            var configCreation = await _mediator.Send(new ConfigCreatorCommand());

            if (configCreation.ConfigCreated == ConfigCreated.False)
            {
                // load config if creation if config exists 

                var configResult = await _mediator.Send(new ConfigLoaderCommand());

                // scan config for source paths to archive

                if (configResult.ConfigLoaded)
                {

                    var result = await _mediator.Send(new FolderScannerCommand());

                    result.FileList.Select(async x =>
                    {
                        var processFile = await _mediator.Send(new FileArchiverCommand() { FileName = x });

                        if (!processFile.ArchiveSuccess)
                        {
                            // add to locked list
                        }
                    });




                    // try each locked file {

                    // log result... back off... skip over if retry limit reached

                    // }


                    // report on skipped files
                }
                else
                {
                    Console.WriteLine("There was an issue loading configuration settings... Check config.ini");
                }
            }
            else if (configCreation.ConfigCreated == ConfigCreated.True)
            {
                // user needs to setup their config file after creation


            }


        }
    }
}


