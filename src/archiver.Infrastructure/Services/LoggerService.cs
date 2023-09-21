namespace archiver.Infrastructure.Services
{
    using archiver.Core;
    using archiver.Infrastructure.Interfaces;
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

            try
            {
                using (FileStream fs = new FileStream(LogFilePath, FileMode.Append))
                {
                    var bytes = Encoding.UTF8.GetBytes(textString + "\r\n");
                    await fs.WriteAsync(bytes);
                };
            }
            catch (IOException)
            {

            }
        }

    }
}
