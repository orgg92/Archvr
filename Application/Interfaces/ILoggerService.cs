namespace Application.Interfaces
{
    using System.Threading.Tasks;

    public interface ILoggerService
    {
        Task WriteConsoleMessageToLogFile(string textString);
    }
}