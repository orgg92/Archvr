namespace Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Application.Interfaces;

    public class LoggerService : ILoggerService
    {
        private string LogFilePath;

        public LoggerService()
        {
            LogFilePath = SharedContent.LogName;
        }

        public async Task WriteConsoleMessageToLogFile(string textString)
        {

        }


    }
}
