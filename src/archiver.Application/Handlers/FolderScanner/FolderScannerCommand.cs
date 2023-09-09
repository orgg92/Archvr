namespace archiver.Application.Handlers.FolderScanner
{
    using archiver.Core;
    using MediatR;

    public class FolderScannerCommand : IRequest<FolderScannerResponse> { }

    public class FolderScannerHandler : IRequestHandler<FolderScannerCommand, FolderScannerResponse>
    {
        public async Task<FolderScannerResponse> Handle(FolderScannerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // read directories from DirListFileLocation [i.e. the text file of directories to archive]

                var fr = File.ReadAllLines(ProgramConfig.DirListFileLocation);

                var fileList = new List<string>();

                foreach (var directory in fr)
                {
                    fileList.AddRange(Directory.EnumerateFiles(ProgramConfig.FilePathCreator(ProgramConfig.FormatDriveToStringContext(), directory)));
                }

                return new FolderScannerResponse() { FileList = fileList };

            } catch (Exception e)
            {
                throw new ProgramException() { ErrorCode = ErrorCodes.FOLDER_SCAN, ErrorMessage = e.Message };
            }
        }
    }
}
