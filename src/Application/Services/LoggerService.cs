namespace archiver.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using archiver.Core;

    public class LoggerService : ILoggerService
    {
        private string LogFilePath;

        public LoggerService()
        {
            LogFilePath = SharedContent.LogName;
        }

        public async Task WriteConsoleMessageToLogFile(string textString)
        {
            if (!Directory.Exists(SharedContent.LogPath))
            {
                Directory.CreateDirectory(SharedContent.LogPath);
            }

            //if (!File.Exists(LogFilePath))
            //{
            //    File.Create(LogFilePath);
            //    await Task.Delay(1000);
            //}

            try
            {
                using (FileStream fs = new FileStream(LogFilePath, FileMode.Append))
                {
                    var bytes = Encoding.UTF8.GetBytes(textString + "\r\n");
                    await fs.WriteAsync(bytes);
                };
            } catch (IOException e)
            {

            }

                //File.AppendAllText(LogFilePath, textString + "\r\n");
        }


    }
}
