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
            var test = SharedContent.DirListFileLocation;
            var sourceDirector = Directory.EnumerateFiles(SharedContent.DirListFileLocation);

            return new FolderScannerResponse();
        }
    }
}
