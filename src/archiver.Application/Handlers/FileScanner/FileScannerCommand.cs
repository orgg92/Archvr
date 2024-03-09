namespace archiver.Application.Handlers.FileScanner
{
    using archiver.Application.Handlers.FolderScanner;
    using archiver.Core;
    using archiver.Core.Interfaces;
    using archiver.Core.Services;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class FileScannerCommand : IRequest<FileScannerResponse> { public string Directory { get; set; } }

    public class FileScannerHandler : IRequestHandler<FileScannerCommand, FileScannerResponse>
    {
        private readonly IIOService _ioService;

        public FileScannerHandler(IIOService ioService)
        {
            _ioService = ioService;
        }

        public async Task<FileScannerResponse> Handle(FileScannerCommand request, CancellationToken cancellationToken)
        {

            //return new FileScannerResponse() { FileList = _ioService.ReturnFileList(request.Directory) };
            return new FileScannerResponse() { FileList = Directory.EnumerateFiles(ProgramConfig.FilePathCreator(ProgramConfig.FormatDriveToStringContext(), request.Directory), "*.*", SearchOption.AllDirectories).ToArray() } ;
        }
    }

}
