namespace archiver.Infrastructure.Interfaces
{
    using System.Threading.Tasks;

    public interface ILoggerService
    {
        Task WriteConsoleMessageToLogFile(string textString);
    }
}