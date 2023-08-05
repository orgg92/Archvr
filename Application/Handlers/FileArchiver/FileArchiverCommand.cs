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
                // if source file doesn't exist in destination folder, copy

                // if file exists and source is newer than destination file's last save, overwrite file  

                return new FileArchiverResponse() { ArchiveSuccess = true };
            } catch (Exception e)
            {
                var exception = new ProgramException() { ErrorCode = "ArchiveError", ErrorMessage = "There was an error" };
                return new FileArchiverResponse() { ArchiveSuccess = false, HandlerException = exception };
                    
            }

        }
    }
}
