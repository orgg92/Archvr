namespace archiver.Application.Handlers.FileScanner
{
    public class FileScannerResponse : BaseResponse  
    { 
        public IEnumerable<string> FileList { get; set; }
    }
}
