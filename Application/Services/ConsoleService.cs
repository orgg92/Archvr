namespace Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using Application.Interfaces;

    public class ConsoleService : IConsoleService
    {
        private readonly ILoggerService _loggerService;

        public ConsoleService(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public void PrintLineDecoration()
        {
            // Console.WriteLine()
        }

        public async Task WriteToConsole(string textString)
        {
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
