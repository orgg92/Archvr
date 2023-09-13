namespace archiver.Infrastructure.Interfaces
{
    using archiver.Infrastructure.Services;

    public interface IConsoleService
    {
        string GetUserInput();
        Task WriteToConsole(string textString, LoggingLevel loggingLevel);
    }
}