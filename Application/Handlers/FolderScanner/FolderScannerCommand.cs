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
            var sourceDirector = Directory.EnumerateFiles($"{SharedContent.TargetDrive}/{SharedContent.DirListFileLocation}");

            return new FolderScannerResponse();
        }
    }
}
