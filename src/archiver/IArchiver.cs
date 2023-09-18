namespace archiver
{
    using archiver.Application.Handlers.ConfigCreator;
    using archiver.Application.Handlers.ConfigLoader;
    using archiver.Application.Handlers.FolderScanner;
    using System.Threading.Tasks;

    public interface IArchiver
    {
        Task<ConfigCreatorResponse> CreateConfig();
        Task Initialize();
        Task<ConfigLoaderResponse> LoadConfig();
        Task<FolderScannerResponse> ScanDirectories();
        Task ProcessFileList(IEnumerable<string> fileList, bool retryMode = false);
    }
}