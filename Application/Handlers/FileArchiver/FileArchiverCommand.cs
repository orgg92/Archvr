namespace Application.Handlers.FileArchiver
{
    using MediatR;

    public class FileArchiverCommand : IRequest<FileArchiverResponse>
    {
        public string FileName { get; set; }
    }

    public class FileArchiverHandler : IRequestHandler<FileArchiverCommand, FileArchiverResponse>
    {
        public async Task<FileArchiverResponse> Handle(FileArchiverCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // if SOURCE VERSION OF THE DESTINATION FILE doesn't exist
                // OR
                // the DESTINATION FILE last modified is before the SOURCE FILE'S last modified
                var destinationDirectory = SharedContent.ReplaceDriveToArchiveContext(SharedContent.DestinationDrive.ToCharArray()[0], new DirectoryInfo(request.FileName).Parent.ToString());

                // check - if not exists, create
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                // get full destination filepath and create if not exists
                var destPath = SharedContent.ReplaceDriveToArchiveContext(SharedContent.DestinationDrive.ToCharArray()[0], request.FileName);
                var srcPath = request.FileName;

                if (!File.Exists(destPath) ) // || (File.GetLastWriteTimeUtc(srcPath) > File.GetLastWriteTimeUtc(destPath)) )
                {
                    var destPath_ArchiveContext = SharedContent.GetFullArchiveAndFilePath(request.FileName);
                    File.Copy(srcPath, destPath_ArchiveContext);
                }

                return new FileArchiverResponse() { ArchiveSuccess = true };
            } catch (Exception e)
            {
                var exception = new ProgramException() { ErrorCode = "ArchiveError", ErrorMessage = "There was an error" };
                return new FileArchiverResponse() { ArchiveSuccess = false, HandlerException = exception };
                    
            }

        }
    }
}
