namespace archiver.Application.Handlers.FolderScanner
{
    using System.Collections.Generic;

    public class FolderScannerResponse : BaseResponse
    {
        public IEnumerable<string> FileList { get; set; }
    }
}
