namespace Application.Handlers.FolderScanner
{
    using MediatR;

    public class FolderScannerCommand : IRequest<FolderScannerResponse>
    {
    }

    public class FolderScannerHandler : IRequestHandler<FolderScannerCommand, FolderScannerResponse>
    {
        public async Task<FolderScannerResponse> Handle(FolderScannerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // read directories from DirListFileLocation

                var fr = File.ReadAllLines(SharedContent.DirListFileLocation);

                var fileList = new List<string>();

                foreach (var directory in fr)
                {
                    fileList.AddRange(Directory.EnumerateFiles(SharedContent.FilePathCreator(SharedContent.FormatDriveToStringContext(), directory)));
                }



                return new FolderScannerResponse() { FileList = fileList };
            } catch (Exception e)
            {
                throw new ProgramException() { ErrorCode = "FOLDER_SCAN", ErrorMessage = e.Message };
            }
        }
    }
}
