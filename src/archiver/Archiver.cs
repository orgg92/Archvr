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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Archiver : IArchiver
    {

        public IMediator _mediator;
        public IConsoleService _consoleService;

        public static List<string> _lockedFiles;

        public Archiver(IMediator mediator, IConsoleService consoleService)
        {
            _mediator = mediator;
            _consoleService = consoleService;

            _lockedFiles = new List<string>();
        }

        public virtual async Task<ConfigCreatorResponse> CreateConfig()
        {
            return await _mediator.Send(new ConfigCreatorCommand());
        }

        public virtual async Task<ConfigLoaderResponse> LoadConfig()
        {
            return await _mediator.Send(new ConfigLoaderCommand());
        }

        public virtual async Task<FolderScannerResponse> ScanDirectories()
        {
            return await _mediator.Send(new FolderScannerCommand());
        }

        public virtual async Task<FileArchiverResponse> ArchiveFile(string fileName, int fileNumber = 0, int totalFiles = 0)
        {
            return await _mediator.Send(new FileArchiverCommand() { FileName = fileName, FileNumber = fileNumber, TotalFiles = totalFiles });
        }

        public async Task Initialize()
        {

            await _consoleService.ClearConsole();
            await _consoleService.SetConsoleSize();

            await _consoleService.WriteToConsole(ProgramConfig.ResponsiveSpacer, Infrastructure.Services.LoggingLevel.BASIC_MESSAGES);

            var configCreation = await CreateConfig();

            if (configCreation.ConfigCreated == ConfigCreated.False)
            {
                var configResult = await LoadConfig();

                if (configResult.ConfigLoaded)
                {
                    var result = await ScanDirectories();

                    await _consoleService.WriteToConsole(SharedContent.ReturnDateFormattedConsoleMessage($"Found {result.FileList.Count()} file(s)"), Infrastructure.Services.LoggingLevel.BASIC_MESSAGES);

                    await ProcessFileList(result.FileList);

                    // Handle files which were locked or unavailable during the first pass
                    if (_lockedFiles.Any())
                    {
                        foreach (var file in _lockedFiles)
                        {
                            var processFile = await ArchiveFile(file);
                        }
                    }
                }

                else
                {
                    await _consoleService.WriteToConsole("There was an issue loading configuration settings... Check config.ini", Infrastructure.Services.LoggingLevel.BASIC_MESSAGES);
                }
            }

            else if (configCreation.ConfigCreated == ConfigCreated.True)
            {
                await _consoleService.WriteToConsole("Config created but requires user setup", Infrastructure.Services.LoggingLevel.BASIC_MESSAGES);
                // user needs to setup their config file after creation
            }

            await _consoleService.WriteToConsole(SharedContent.ReturnDateFormattedConsoleMessage($"Archiving completed without issue"), Infrastructure.Services.LoggingLevel.BASIC_MESSAGES);

            await _consoleService.WriteToConsole(ProgramConfig.ResponsiveSpacer, Infrastructure.Services.LoggingLevel.BASIC_MESSAGES);
        }

        public async Task ProcessFileList(IEnumerable<string> fileList, bool retryMode = false)
        {
            var i = 0;

            await _consoleService.WriteToConsole($"{SharedContent.ReturnFormattedDateTimeToString()} {SharedContent.ReturnMessageForHandler(HandlerNames.FileArchiverCommand.ToString())}", Infrastructure.Services.LoggingLevel.BASIC_MESSAGES);

            foreach (var file in fileList)
            {

                var fileProgressMeter = $"[{i}/{fileList.Count()}]";

                var message = $"{fileProgressMeter} {new String('-', 25 - fileProgressMeter.Count())}> {file}";

                await _consoleService.WriteToConsole(message, Infrastructure.Services.LoggingLevel.FILE_STATUS);

                //var processFile = await _mediator.Send(new FileArchiverCommand() { FileName = file, FileNumber = i + 1, TotalFiles = fileList.Count() });
                var processFile = await ArchiveFile(file, i + 1, fileList.Count());

                if (!processFile.ArchiveSuccess && !retryMode)
                {
                    _lockedFiles.Add(file);

                }
                else if (!processFile.ArchiveSuccess && retryMode)
                {
                    await _consoleService.WriteToConsole($"Archiving file: {file} failed after retrying", Infrastructure.Services.LoggingLevel.BASIC_MESSAGES);
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
