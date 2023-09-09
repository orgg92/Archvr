namespace archiver.Application.Services
{
    using Application.Interfaces;
    using System;
    using System.Threading.Tasks;

    public class ConsoleService : IConsoleService
    {
        private readonly ILoggerService _loggerService;

        public ConsoleService(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public async Task WriteToConsole(string textString)
        {
            // When writing to console, also write to log file (for headless/service mode)
            Console.WriteLine(textString);
            await WriteToLogFile(textString);
        }

        public string GetUserInput()
        {
            return Console.ReadLine();
        }

        public async Task WriteToLogFile(string textString)
        {
           await _loggerService.WriteConsoleMessageToLogFile(textString);
        }

    }
}
