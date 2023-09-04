namespace archiver.Infrastructure.Interfaces
{
    public interface IConsoleService
    {
        string GetUserInput();
        Task WriteToConsole(string textString);
    }
}