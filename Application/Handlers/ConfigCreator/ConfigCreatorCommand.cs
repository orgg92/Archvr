namespace Application.Handlers.ConfigCreator
{
    using MediatR;

    public class ConfigCreatorCommand : IRequest<ConfigCreatorResponse>
    {

    }

    public class ConfigCreatorHandler : IRequestHandler<ConfigCreatorCommand, ConfigCreatorResponse>
    {
        public string[] argParameters = new string[1];

        public string[] baseConfigValues = new string[]
        {
            "TARGET_DRIVE=''",
            "DESTINATION_DRIVE=''",
            "RETRY_COUNT=''",
            "LOG_PROGRESS_TO_CONSOLE='true'",
            $"DIRFILELOCATION='{SharedContent.FilePathCreator(SharedContent.ConfigDirectoryPath, "directory-list.txt")}'",
            "OUTPUT_LOCATION=''",
            "CONSOLE_HEIGHT='25'",
            "CONSOLE_WIDTH='100'"
        };

        public Tuple<string, string>[] ConfigLocations;

        public ConfigCreatorHandler()
        {
            SharedContent.CurrentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var logPath = SharedContent.FilePathCreator(SharedContent.CurrentPath, SharedContent.LogPath);
            var configPath = SharedContent.FilePathCreator(SharedContent.CurrentPath, SharedContent.ConfigDirectoryPath);

            this.ConfigLocations = new Tuple<string, string>[] {
                new Tuple<string, string> (logPath,   SharedContent.LogName),
                new Tuple<string, string> (configPath,  SharedContent.DirListFileLocation), // only need 
                new Tuple<string, string> (configPath,  SharedContent.ConfigFullPath )
            };
        }

        public async Task<ConfigCreatorResponse> Handle(ConfigCreatorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // if file path exists then return false

                return this.CheckConfigExists()
                    ? new ConfigCreatorResponse() { ConfigCreated = ConfigCreated.False }
                    : WriteNewConfigFile();

            } catch (Exception e)
            {
                throw new ProgramException() { ErrorCode = "CONFIG_CREATION_ERROR", ErrorMessage = e.Message };
            }

        }

        public bool CheckConfigExists()
        {
            return Directory.Exists(SharedContent.ConfigDirectoryPath);          
        }

        public ConfigCreatorResponse WriteNewConfigFile()
        {
            

            for (int i = 0; i < this.ConfigLocations.Length; i++)
            {
                var filedir = this.ConfigLocations[i].Item1;
                var filePath = this.ConfigLocations[i].Item2;

                // if directory doesn't exist, create it, if file doesn't exist create that too
                if (!Directory.Exists(filedir))
                {
                    Directory.CreateDirectory(filedir);
                }

                if (!File.Exists(filePath))
                {

                    if (filePath.Contains("config.ini")) {
                        using (FileStream fs = File.Create(filePath))
                        {
                            using (StreamWriter sw = new StreamWriter(fs))
                            {
                                foreach (var config in baseConfigValues)
                                { 

                                    // Create config file with boilerplate values
                                    {
                                        sw.WriteLine(config);
                                    }
                                }
                            }
                        } 
                    } else {

                        // create blank files for writing later

                        File.Create(filePath);
                    }
                }
            }

            return new ConfigCreatorResponse() { ConfigCreated = ConfigCreated.True };
        }
    }
}
