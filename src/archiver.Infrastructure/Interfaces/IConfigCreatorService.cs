namespace archiver.Infrastructure.Interfaces
{
    public interface IConfigCreatorService
    {
        bool CheckConfigExists();
        void WriteNewConfigFile();
    }
}