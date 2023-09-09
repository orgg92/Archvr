namespace archiver.Application.Services
{
    using Application.Interfaces;
    using archiver.Core;
    using System.Text;
    using System.Threading.Tasks;

    public class LoggerService : ILoggerService
    {
        private string LogFilePath;

        public LoggerService()
        {
            LogFilePath = ProgramConfig.LogName;
        }

        public async Task WriteConsoleMessageToLogFile(string textString)
        {
            if (!Directory.Exists(ProgramConfig.LogPath))
            {
                Directory.CreateDirectory(ProgramConfig.LogPath);
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
