namespace archiver.Application.Services
{
    using archiver.Application.Interfaces;
    using archiver.Core;
    using System;


    public class ConfigService : IConfigService
    {

        private string logPath,
                       configPath;

        public Tuple<string, string>[] ConfigLocations;

        public ConfigService()
        {
            ProgramConfig.CurrentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            logPath = ProgramConfig.FilePathCreator(ProgramConfig.CurrentPath, ProgramConfig.LogPath);
            configPath = ProgramConfig.FilePathCreator(ProgramConfig.CurrentPath, ProgramConfig.ConfigDirectoryPath);

            ConfigLocations = new Tuple<string, string>[] 
            {
                new Tuple<string, string> (logPath,   ProgramConfig.LogName),
                new Tuple<string, string> (configPath,  ProgramConfig.DirListFileLocation), // only need 
                new Tuple<string, string> (configPath,  ProgramConfig.ConfigFullPath )
            };
        }

        public bool CheckConfigExists()
        {
            return Directory.Exists(ProgramConfig.ConfigDirectoryPath) && File.Exists(ProgramConfig.ConfigFullPath);
        }

        //check file has been modified since it's creation, otherwise the file will only contain default config template so no point continuing
        public bool CheckConfigHasBeenTouched()
        {
            return File.GetCreationTimeUtc(ProgramConfig.ConfigFullPath) != File.GetLastWriteTimeUtc(ProgramConfig.ConfigFullPath);
        }

        public bool CheckConfigDirectoriesExists()
        {
            return Directory.Exists(ProgramConfig.ConfigDirectoryPath) && Directory.Exists(ProgramConfig.LogPath);
        }

        public void WriteNewConfigFile()
        {
            for (int i = 0; i < this.ConfigLocations.Length; i++)
            {
                var filedir = this.ConfigLocations[i].Item1;
                var filePath = this.ConfigLocations[i].Item2;

                // if directory doesn't exist, create it, if file doesn't exist create that too - this ensures all prerequisite files are created on startup
                if (!Directory.Exists(filedir))
                {
                    Directory.CreateDirectory(filedir);
                }

                if (!File.Exists(filePath))
                {
                    if (filePath.Contains("config.ini"))
                    {
                        using (FileStream fs = File.Open(filePath, FileMode.OpenOrCreate))
                        {
                            using (StreamWriter sw = new StreamWriter(fs))
                            {
                                var config = new BaseConfigValues();

                                foreach (var value in config.Values)
                                {
                                    // Create config file with boilerplate values
                                    {
                                        sw.WriteLine(value.ReturnConfigReadyString());
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // if it's not config.ini: create blank file for writing later

                        File.Create(filePath);
                    }
                }
            }
        }

        public void LoadConfig()
        {
            int i = 0;
            List<WriteConfigSettingModel> configSettings = new List<WriteConfigSettingModel>();
            foreach (string line in File.ReadLines($"{ProgramConfig.ConfigFullPath}"))
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
            ProgramConfig.ConsoleHeight = SelectConfigValue<int>("CONSOLE_HEIGHT") == 0 ? 25 : SelectConfigValue<int>("CONSOLE_HEIGHT");
            ProgramConfig.ConsoleWidth = SelectConfigValue<int>("CONSOLE_WIDTH") == 0 ? 100 : SelectConfigValue<int>("CONSOLE_WIDTH");
            ProgramConfig.ArchiveFolderName = SelectConfigValue<string>("ARCHIVE_FOLDER_NAME");
            ProgramConfig.LogLevel = SelectConfigValue<int>("LOG_LEVEL") ?? 0;

            ProgramConfig.ResponsiveSpacer = new String('*', ProgramConfig.ConsoleWidth);
        }

        public static dynamic SelectConfigValue<T>(string configSetting)
        {
            bool boolResult;
            int intResult;

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

        public virtual void SetConsoleSize()
        {
            if (System.Environment.OSVersion.Platform.ToString().ToLower().Contains("win"))
            {
                Console.SetWindowSize(ProgramConfig.ConsoleWidth, ProgramConfig.ConsoleHeight);
            }
            else
            {
                ProgramConfig.ConsoleHeight = Console.WindowHeight;
                ProgramConfig.ConsoleWidth = Console.WindowWidth;
            }
        }
    }
}
