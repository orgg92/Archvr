namespace archiver.Application.Handlers.FolderScanner
{
    using archiver.Application.Interfaces;
    using archiver.Core;
    using archiver.Core.Enum;
    using archiver.Infrastructure.Interfaces;
    using MediatR;

    public class FolderScannerCommand : IRequest<FolderScannerResponse> { }

    public class FolderScannerHandler : IRequestHandler<FolderScannerCommand, FolderScannerResponse>
    {
        private readonly IIOService _ioService;
        private readonly IConsoleService _consoleService;
        private List<string> _fileList;

        public FolderScannerHandler(IIOService ioService, IConsoleService consoleService)
        {
            _ioService = ioService;
            _consoleService = consoleService;   

            _fileList = new List<string>();
        }

        public async Task<FolderScannerResponse> Handle(FolderScannerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // read directories from DirListFileLocation [i.e. the text file of directories to archive] and check the folders exist
                var folderList = _ioService.ReadConfigFileDirectoryList();

                foreach (var folder in folderList)
                {
                    if (_ioService.CheckDirectoryExists(folder))
                    {
                        _fileList.AddRange(_ioService.ReturnFileList(folder));
                    } else {
                        // inform the folder does not exist and will not be archived
                       await _consoleService.WriteToConsole(
                           SharedContent.ReturnDateFormattedConsoleMessage($"Directory does not exist and will be skipped: {folder}"),
                           Infrastructure.Services.LoggingLevel.BASIC_MESSAGES
                        );
                    }
                }

                return new FolderScannerResponse() { FileList = _fileList };

            } catch (Exception e)
            {
                throw new ProgramException() { ErrorCode = ErrorCodes.FOLDER_SCAN, ErrorMessage = e.Message };
            }
        }
    }
}
