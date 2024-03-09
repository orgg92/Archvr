namespace archiver
{
    using archiver.Application.Handlers.ConfigCreator;
    using archiver.Application.Handlers.ConfigLoader;
    using archiver.Application.Handlers.FileArchiver;
    using archiver.Application.Handlers.FileScanner;
    using archiver.Application.Handlers.FolderScanner;
    using archiver.Core;
    using archiver.Core.Enum;
    using archiver.Core.Extensions;
    using archiver.Infrastructure.Interfaces;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    public class Archiver
    {
        private IMediator _mediator;
        private IConsoleService _consoleService;

        public static List<string> _fileList;
        public static List<string> _lockedFiles;
        private int RetryCount = ProgramConfig.RetryCount,
                    RetryIteration = 0;

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

        public virtual async Task<FolderScannerResponse> ScanConfigForDirectories()
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
            await _consoleService.WriteToConsole(ProgramConfig.ResponsiveSpacer, LoggingLevel.BASIC_MESSAGES);

            var configCreation = await CreateConfig();

            if (configCreation.ConfigCreated == ConfigCreated.False)
            {
                var configResult = await LoadConfig();

                if (configResult.ConfigLoaded)
                {
                    // get list of config dirs
                    var directoryScan = await ScanConfigForDirectories();

                    ThreadedFileScan(directoryScan.FolderList);

                    // process each file list separately in a threaded request

                    // loop through folder list and start new scan per directory
                    await _consoleService.WriteToConsole(SharedContent.ReturnDateFormattedConsoleMessage($"Found {_fileList.Count()} file(s)"), LoggingLevel.BASIC_MESSAGES);

                    await ProcessFileList(_fileList);

                    // Handle files which were locked or unavailable during the first pass, if none program runs to completion
                    await ProcessLockedFiles();

                }

                else
                {
                    await _consoleService.WriteToConsole("There was an issue loading configuration settings... Check config.ini", LoggingLevel.BASIC_MESSAGES);
                }
            }

            else if (configCreation.ConfigCreated == ConfigCreated.True)
            {
                // user needs to setup their config file after creation
                await _consoleService.WriteToConsole("Config created but requires user setup", LoggingLevel.BASIC_MESSAGES);
            }

            await _consoleService.WriteToConsole(SharedContent.ReturnDateFormattedConsoleMessage($"Archiving completed without issue"), LoggingLevel.BASIC_MESSAGES);
            await _consoleService.WriteToConsole(ProgramConfig.ResponsiveSpacer, LoggingLevel.BASIC_MESSAGES);
        }

        public async Task ProcessLockedFiles()
        {
            if (_lockedFiles.Any())
            {
                if (RetryIteration <= RetryCount)
                {
                    // attempt to archive files again
                    await ProcessFileList(_lockedFiles, true);
                    RetryIteration++;
                }
            }
        }

        public void ThreadedFileScan(IEnumerable<string> folderList)
        {
            // speeds up file scanning process dramatically, especially when dealing with a large amount of files

            _fileList = new List<string>();

            var threadList = new List<Thread>();
            var numberOfThreads = Process.GetCurrentProcess().Threads.Count;

            for(int i=0; i < folderList.Count(); i++)
            {
                threadList.Add(new Thread(() => FileScan(folderList.ToArray()[i])));
                threadList[i].Start();
                Thread.Sleep(150);
            }

            threadList.WaitAll();
            threadList.Clear();


        }

        public static void FileScan(object folder)
        {
            _fileList.AddRange(Directory.EnumerateFiles(ProgramConfig.FilePathCreator(ProgramConfig.FormatDriveToStringContext(), (string)folder), "*.*", SearchOption.AllDirectories).ToArray());
        }

        public async Task ProcessFileList(IEnumerable<string> fileList, bool retryMode = false)
        {
            var i = 0;

            await _consoleService.WriteToConsole($"{SharedContent.ReturnFormattedDateTimeToString()} {SharedContent.ReturnMessageForHandler(HandlerNames.FileArchiverCommand.ToString())}", LoggingLevel.BASIC_MESSAGES);

            foreach (var file in fileList)
            {

                var fileProgressMeter = $"[{i}/{fileList.Count() - 1}]";

                // message to reflect the destination filepath of the target file
                var message = $"{fileProgressMeter} " +
                    $"{new String('-', 25 - fileProgressMeter.Count() - 1)}> " +
                    $"{file} \r\n" +
                    $"{new String('-', 25)}> " +
                    $"{ProgramConfig.GetFullArchiveAndFilePath(file)}";

                await _consoleService.WriteToConsole(message, LoggingLevel.FILE_STATUS);

                var processFile = await ArchiveFile(file, i + 1, fileList.Count() - 1);

                if (!processFile.ArchiveSuccess && !retryMode)
                {
                    _lockedFiles.Add(file);

                }
                else if (!processFile.ArchiveSuccess && retryMode)
                {
                    await _consoleService.WriteToConsole($"Archiving file: {file} failed after retrying", LoggingLevel.BASIC_MESSAGES);
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
