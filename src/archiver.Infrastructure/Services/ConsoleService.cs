namespace archiver.Infrastructure.Services
{
    using archiver.Core;
    using archiver.Infrastructure.Interfaces;
    using System;
    using System.Threading.Tasks;

    public class ConsoleService : IConsoleService
    {
        private readonly ILoggerService _loggerService;

        public ConsoleService(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public async Task WriteToConsole(string textString, LoggingLevel loggingLevel, ConsoleColor? colour = ConsoleColor.Gray)
        {
            // if message doesn't meet the logging level swallow the invocation and don't write to console
            if (ShouldMessageBeLogged(loggingLevel))
            {
                Console.WriteLine(textString);
                await WriteToLogFile(textString);
            }
        }

        public string GetUserInput()
        {
            return Console.ReadLine();
        }

        public async Task WriteToLogFile(string textString)
        {
            await _loggerService.WriteConsoleMessageToLogFile(textString);
        }

        public bool ShouldMessageBeLogged(LoggingLevel loggingLevel)
        {
            // evaluate whether the Log Level defined in program config (parsed from config file) matches the logging level passed into the method call 
            return ProgramConfig.LogLevel >= (int)loggingLevel;
        }

        public async Task ClearConsole()
        {
            Console.Clear();
        }

        public async Task SetConsoleSize()
        {
            ProgramConfig.ConsoleWidth = Console.WindowWidth;
            ProgramConfig.ResponsiveSpacer = new String('*', ProgramConfig.ConsoleWidth);
        }

    }
}
