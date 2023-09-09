namespace archiver.Application.Services
{
    using archiver.Application.Interfaces;
    using archiver.Core;
    using System;


    public class ConfigCreatorService : IConfigCreatorService
    {

        private string logPath,
                       configPath;

        public Tuple<string, string>[] ConfigLocations;

        public ConfigCreatorService()
        {
            ProgramConfig.CurrentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            logPath = ProgramConfig.FilePathCreator(ProgramConfig.CurrentPath, ProgramConfig.LogPath);
            configPath = ProgramConfig.FilePathCreator(ProgramConfig.CurrentPath, ProgramConfig.ConfigDirectoryPath);

            this.ConfigLocations = new Tuple<string, string>[] {
                new Tuple<string, string> (logPath,   ProgramConfig.LogName),
                new Tuple<string, string> (configPath,  ProgramConfig.DirListFileLocation), // only need 
                new Tuple<string, string> (configPath,  ProgramConfig.ConfigFullPath )
            };
        }

        public bool CheckConfigExists()
        {
            return Directory.Exists(ProgramConfig.ConfigDirectoryPath);
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
                        using (FileStream fs = File.Create(filePath))
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

                        // create blank files for writing later

                        File.Create(filePath);
                    }
                }
            }
        }
    }
}
