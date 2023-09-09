namespace archiver.Application.Handlers.ConfigLoader
{
    using archiver.Core;
    using MediatR;

    public class ConfigLoaderCommand : IRequest<ConfigLoaderResponse> { }

    public class ConfigLoaderHandler : IRequestHandler<ConfigLoaderCommand, ConfigLoaderResponse>
    {

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

        //check file has been modified since it's creation, otherwise the file will only contain default config template so no point trying to load
        public bool CheckConfigHasBeenTouched()
        {
            return File.GetCreationTimeUtc(ProgramConfig.ConfigFullPath) != File.GetLastWriteTimeUtc(ProgramConfig.ConfigFullPath);
        }

        public bool CheckConfigExists()
        {
            return Directory.Exists(ProgramConfig.ConfigDirectoryPath) && Directory.Exists(ProgramConfig.LogPath);
        }

        public static string SelectConfigValue(string configSetting)
        {
            return ProgramConfig.configValues.Where(x => x.Name == configSetting).First().Value;
        }

        public void LoadConfig()
        {
            int i = 0;
            List<WriteConfigSettingModel> configSettings = new List<WriteConfigSettingModel>();
            foreach (string line in System.IO.File.ReadLines($"{ProgramConfig.ConfigFullPath}"))
            {
                var setting = line.Split("=");

                configSettings.Add(new WriteConfigSettingModel()
                {
                    Name = setting[0],
                    Value = setting[1].Replace("'", "")
                });

                i++;
            }

            // set global values for use throughout the application
            ProgramConfig.configValues = configSettings.ToArray();

            ProgramConfig.TargetDrive = SelectConfigValue("TARGET_DRIVE");
            ProgramConfig.DestinationDrive = SelectConfigValue("DESTINATION_DRIVE");
            ProgramConfig.RetryCount = SelectConfigValue("RETRY_COUNT");
            ProgramConfig.LogProgressToConsole = Boolean.TryParse(SelectConfigValue("LOG_PROGRESS_TO_CONSOLE"), out var result) ? result : false;
            ProgramConfig.DirListFileLocation = SelectConfigValue("DIRFILE_LOCATION");
            ProgramConfig.OutputLocation = SelectConfigValue("OUTPUT_LOCATION");
            ProgramConfig.ConsoleHeight = int.TryParse(SelectConfigValue("CONSOLE_HEIGHT"), out int ch) ? ch : 25;
            ProgramConfig.ConsoleWidth = int.TryParse(SelectConfigValue("CONSOLE_WIDTH"), out int cw) ? cw : 100;
            ProgramConfig.ArchiveFolderName = SelectConfigValue("ARCHIVE_FOLDER_NAME");

            SetConsoleSize();

            ProgramConfig.ResponsiveSpacer = new String('*', ProgramConfig.ConsoleWidth);
        }


        // allows program to run 
        public void LoadDefaultValues()
        {

        }

        public void SetConsoleSize()
        {
            if (System.Environment.OSVersion.Platform.ToString().ToLower().Contains("win"))
            {
                Console.SetWindowSize(ProgramConfig.ConsoleWidth, ProgramConfig.ConsoleHeight);
            } else
            {
                ProgramConfig.ConsoleHeight = Console.WindowHeight;
                ProgramConfig.ConsoleWidth = Console.WindowWidth;
            }
        }
    }
}
