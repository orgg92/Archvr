namespace archiver.Application.Handlers.FolderScanner
{
    using archiver.Application.Interfaces;
    using archiver.Core;
    using MediatR;

    public class FolderScannerCommand : IRequest<FolderScannerResponse> { }

    public class FolderScannerHandler : IRequestHandler<FolderScannerCommand, FolderScannerResponse>
    {
        private readonly IIOService _ioService;
        private readonly IConsoleService _consoleService;

        public FolderScannerHandler(IIOService ioService, IConsoleService consoleService)
        {
            _ioService = ioService;
            _consoleService = consoleService;
        }

        public async Task<FolderScannerResponse> Handle(FolderScannerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // read directories from DirListFileLocation [i.e. the text file of directories to archive]

                var fileList = _ioService.ReturnFileList();

                await _consoleService.WriteToConsole(SharedContent.ReturnDateFormattedConsoleMessage($"Found {fileList.Count()} file(s)"));

                return new FolderScannerResponse() { FileList = fileList };

            } catch (Exception e)
            {
                throw new ProgramException() { ErrorCode = ErrorCodes.FOLDER_SCAN, ErrorMessage = e.Message };
            }
        }
    }
}
