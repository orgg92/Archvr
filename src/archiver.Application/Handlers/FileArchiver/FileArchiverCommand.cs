namespace archiver.Application.Handlers.FileArchiver
{
    using Application.Interfaces;
    using archiver.Core;
    using archiver.Core.Enum;
    using archiver.Infrastructure.Interfaces;
    using MediatR;

    public class FileArchiverCommand : IRequest<FileArchiverResponse>
    {
        public string FileName { get; set; }
        public bool? IsRetry { get; set; }
        public int FileNumber { get; set; }
        public int TotalFiles { get; set; }
    }

    public class FileArchiverHandler : IRequestHandler<FileArchiverCommand, FileArchiverResponse>
    {
        private readonly IConsoleService _consoleService;
        private readonly IIOService _ioService;

        public FileArchiverHandler(IConsoleService consoleService, IIOService ioService)
        {
            _consoleService = consoleService;
            _ioService = ioService;
        }

        public async Task<FileArchiverResponse> Handle(FileArchiverCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var destinationDirectory = _ioService.GetDestinationDirectory(request.FileName);

                // check - if not exists, create
                if (!_ioService.CheckDirectoryExists(destinationDirectory))
                {
                    _ioService.CreateDirectory(destinationDirectory);
                }

                // get full destination filepath and create if not exists
                var destPath = $"{destinationDirectory}\\{new DirectoryInfo(request.FileName).Name}";
                var srcPath = request.FileName;

                // if SOURCE VERSION OF THE DESTINATION FILE doesn't exist
                // OR
                // the DESTINATION FILE last modified is before SOURCE FILE last modified

                // Debug, remove after || 
                if (_ioService.CheckIfFileShouldBeUpdated(srcPath, destPath) )
                {
                    if (ProgramConfig.LogLevel > 0)
                        await _consoleService.WriteToConsole("Source file is newer than archive file... Overwriting", Infrastructure.Services.LoggingLevel.BASIC_MESSAGES);

                    _ioService.CopyFile(srcPath, destPath);
                }
                else if (ProgramConfig.LogLevel > 0)
                {
                    await _consoleService.WriteToConsole($"Skipping file", Infrastructure.Services.LoggingLevel.BASIC_MESSAGES);
                }


                return new FileArchiverResponse() { ArchiveSuccess = true };

            } catch (Exception e)
            {
                var exception = new ProgramException() { ErrorCode = ErrorCodes.ARCHIVE_ERROR, ErrorMessage = SharedContent.ReturnErrorMessageForErrorCode(ErrorCodes.ARCHIVE_ERROR.ToString()).Replace("{0}", request.FileName ) };

                return new FileArchiverResponse() { ArchiveSuccess = false, HandlerException = exception };    
            }

        }
    }
}
