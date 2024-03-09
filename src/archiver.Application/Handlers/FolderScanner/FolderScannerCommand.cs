namespace archiver.Application.Handlers.FolderScanner
{
    using archiver.Core;
    using archiver.Core.Enum;
    using archiver.Core.Interfaces;
    using archiver.Infrastructure.Interfaces;
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

            //_fileList = new List<string>();
        }

        public async Task<FolderScannerResponse> Handle(FolderScannerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // read directories from DirListFileLocation [i.e. the text file of directories to archive] and check the folders exist
                var folderList = _ioService.ReadConfigFileDirectoryList().ToList();

                foreach (var folder in folderList)
                {
                    if (!_ioService.CheckDirectoryExists(folder))
                    {
                        folderList.Remove(folder);
                        // inform the folder does not exist and will not be archived
                        await _consoleService.WriteToConsole(
                            SharedContent.ReturnDateFormattedConsoleMessage($"Directory does not exist and will be skipped: {folder}"),
                            LoggingLevel.BASIC_MESSAGES
                         );
                    }
                }

                return new FolderScannerResponse() { FolderList = folderList };

            }
            catch (Exception e)
            {
                throw new ProgramException() { ErrorCode = ErrorCodes.FOLDER_SCAN, ErrorMessage = e.Message };
            }
        }
    }
}
