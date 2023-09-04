namespace archiver.Application.Handlers.FileArchiver
{
    using Application.Interfaces;
    using archiver.Core;
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

        public FileArchiverHandler(IConsoleService consoleService)
        {
            _consoleService = consoleService;
        }

        public async Task<FileArchiverResponse> Handle(FileArchiverCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var fileProgressMeter = $"[{request.FileNumber}/{request.TotalFiles}]";

                var message = $"{fileProgressMeter} {new String('-',25 - fileProgressMeter.Count())}> {request.FileName}";

                await _consoleService.WriteToConsole(message);

                var destinationDirectory = SharedContent.ReplaceDriveToArchiveContext(SharedContent.DestinationDrive.ToCharArray()[0], new DirectoryInfo(request.FileName).Parent.ToString());

                // check - if not exists, create
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                // get full destination filepath and create if not exists
                var destPath = $"{destinationDirectory}\\{new DirectoryInfo(request.FileName).Name}";
                var srcPath = request.FileName;

                // if SOURCE VERSION OF THE DESTINATION FILE doesn't exist
                // OR
                // the DESTINATION FILE last modified is before the SOURCE FILE'S last modified

                if (!File.Exists(destPath) )  // < ** DEBUG REMOVE ** // || (File.GetLastWriteTimeUtc(srcPath) > File.GetLastWriteTimeUtc(destPath)) )
                {
                    File.Copy(srcPath, destPath);
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
