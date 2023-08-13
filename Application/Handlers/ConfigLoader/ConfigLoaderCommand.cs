namespace Application.Handlers.ConfigLoader
{
    using MediatR;
    using System.ComponentModel;

    public class ConfigLoaderCommand : IRequest<ConfigLoaderResponse>
    {

    }

    public class ConfigLoaderHandler : IRequestHandler<ConfigLoaderCommand, ConfigLoaderResponse>
    {
        public ConfigLoaderHandler()
        {

        }

        public async Task<ConfigLoaderResponse> Handle(ConfigLoaderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (CheckConfigExists() && CheckConfigHasBeenTouched())
                {
                    LoadConfig();

                    return new ConfigLoaderResponse() { ConfigLoaded = true };
                }
                else
                {
                    return new ConfigLoaderResponse() { ConfigLoaded = false };
                }
            } catch (Exception e)
            {
                throw new ProgramException() { ErrorCode = ErrorCodes.CONFIG_LOAD_ERROR, ErrorMessage = e.Message };
            }


        }

        //check file has been modified since it's creation, otherwise the file will only contain the default config template so no point loading
        public bool CheckConfigHasBeenTouched()
        {
            return File.GetCreationTimeUtc(SharedContent.ConfigFullPath) != File.GetLastWriteTimeUtc(SharedContent.ConfigFullPath);
        }

        public bool CheckConfigExists()
        {
            return Directory.Exists(SharedContent.ConfigDirectoryPath) && Directory.Exists(SharedContent.LogPath);
        }

        public static string SelectConfigValue(string configSetting)
        {
            return SharedContent.configValues.Where(x => x.Name == configSetting).First().Value;
        }

        public void LoadConfig()
        {
            int i = 0;
            List<ConfigSetting> configSettings = new List<ConfigSetting>();
            foreach (string line in System.IO.File.ReadLines($"{SharedContent.ConfigFullPath}"))
            {
                var setting = line.Split("=");

                configSettings.Add(new ConfigSetting()
                {
                    Name = setting[0],
                    Value = setting[1].Replace("'", "")
                });

                i++;
            }

            SharedContent.configValues = configSettings.ToArray();

            SharedContent.TargetDrive = SelectConfigValue("TARGET_DRIVE");
            SharedContent.DestinationDrive = SelectConfigValue("DESTINATION_DRIVE");
            SharedContent.RetryCount = SelectConfigValue("RETRY_COUNT");
            SharedContent.LogProgressToConsole = Boolean.Parse(SelectConfigValue("LOG_PROGRESS_TO_CONSOLE"));
            SharedContent.DirListFileLocation = SelectConfigValue("DIRFILELOCATION");
            SharedContent.OutputLocation = SelectConfigValue("OUTPUT_LOCATION");
            SharedContent.ConsoleHeight = int.TryParse(SelectConfigValue("CONSOLE_HEIGHT"), out int ch) ? ch : 25;
            SharedContent.ConsoleWidth = int.TryParse(SelectConfigValue("CONSOLE_WIDTH"), out int cw) ? cw : 100;
            SharedContent.ArchiveFolderName = SelectConfigValue("ARCHIVE_FOLDER_NAME");

            SetConsoleSize();

            SharedContent.ResponsiveSpacer = new String('*', SharedContent.ConsoleWidth);
        }

        // allows program to run 
        public void LoadDefaultValues()
        {

        }

        public void SetConsoleSize()
        {
            Console.SetWindowSize(SharedContent.ConsoleWidth, SharedContent.ConsoleHeight);
        }
    }
}
