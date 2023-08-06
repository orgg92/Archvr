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

                if (!File.Exists(request.FileName) /*|| File.GetLastWriteTime()*/)
                {

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
