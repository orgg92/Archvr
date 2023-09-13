namespace archiver.Application.Handlers.FolderScanner
{
    using archiver.Application.Interfaces;
    using archiver.Core;
    using archiver.Core.Enum;
    using MediatR;

    public class FolderScannerCommand : IRequest<FolderScannerResponse> { }

    public class FolderScannerHandler : IRequestHandler<FolderScannerCommand, FolderScannerResponse>
    {
        private readonly IIOService _ioService;

        public FolderScannerHandler(IIOService ioService)
        {
            _ioService = ioService;
        }

        public async Task<FolderScannerResponse> Handle(FolderScannerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // read directories from DirListFileLocation [i.e. the text file of directories to archive]

                var fileList = _ioService.ReturnFileList();

                return new FolderScannerResponse() { FileList = fileList };

            } catch (Exception e)
            {
                throw new ProgramException() { ErrorCode = ErrorCodes.FOLDER_SCAN, ErrorMessage = e.Message };
            }
        }
    }
}
