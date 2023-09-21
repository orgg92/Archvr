namespace archiver.Core.Interfaces
{
    public interface IConfigService
    {
        bool CheckConfigExists();
        void WriteNewConfigFile();
        bool CheckConfigHasBeenTouched();
        bool CheckConfigDirectoriesExists();
        void LoadConfig();
        void SetConsoleSize();
    }
}