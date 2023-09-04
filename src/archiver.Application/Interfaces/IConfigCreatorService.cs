namespace archiver.Application.Interfaces
{
    public interface IConfigCreatorService
    {
        bool CheckConfigExists();
        void WriteNewConfigFile();
    }
}