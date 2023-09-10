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

        public static dynamic SelectConfigValue<T>(string configSetting)
        {
            bool boolResult;
            int intResult;
            //return ProgramConfig.configValues.Where(x => x.Name == configSetting).First().Value;
            var value = ProgramConfig.configValues.Where(x => x.Name == configSetting).First().Value;

            // try parse bool
            if (bool.TryParse(value, out boolResult))
            {
                return boolResult;
            }

            // try parse int
            if (int.TryParse(value, out intResult))
            {
                return intResult;
            }

            // if not satisfied return string
            return value;
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

            ProgramConfig.TargetDrive = SelectConfigValue<string>("TARGET_DRIVE");
            ProgramConfig.DestinationDrive = SelectConfigValue<string>("DESTINATION_DRIVE");
            ProgramConfig.RetryCount = SelectConfigValue<int>("RETRY_COUNT");
            ProgramConfig.LogProgressToConsole = SelectConfigValue<bool>("LOG_PROGRESS_TO_CONSOLE");
            ProgramConfig.DirListFileLocation = SelectConfigValue<string>("DIRFILE_LOCATION");
            ProgramConfig.OutputLocation = SelectConfigValue<string>("OUTPUT_LOCATION");
            ProgramConfig.ConsoleHeight = SelectConfigValue<int>("CONSOLE_HEIGHT");
            ProgramConfig.ConsoleWidth = SelectConfigValue<int>("CONSOLE_WIDTH");
            ProgramConfig.ArchiveFolderName = SelectConfigValue<string>("ARCHIVE_FOLDER_NAME");

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
