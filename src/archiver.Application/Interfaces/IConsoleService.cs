namespace archiver.Application.Interfaces
{
    public interface IConsoleService
    {
        string GetUserInput();
        Task WriteToConsole(string textString, int? logLevel = 0);
    }
}