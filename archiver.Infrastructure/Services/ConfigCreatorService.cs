namespace archiver.Infrastructure.Interfaces
{
    using archiver.Core;
    using archiver.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ConfigCreatorService : IConfigCreatorService
    {

        private string logPath,
                       configPath;

        public string[] baseConfigValues = new string[]
        {
            "TARGET_DRIVE=''",
            "DESTINATION_DRIVE=''",
            "RETRY_COUNT=''",
            "LOG_PROGRESS_TO_CONSOLE='true'",
            $"DIRFILELOCATION='{SharedContent.FilePathCreator(SharedContent.ConfigDirectoryPath, "directory-list.txt")}'",
            "OUTPUT_LOCATION=''",
            "CONSOLE_HEIGHT='25'",
            "CONSOLE_WIDTH='100'",
            "ARCHIVE_FOLDER_NAME='Archive'", // this is just the folder name not the full path of desired archive location
            "LOG_LEVEL='0'" // 0-3
        };

        public Tuple<string, string>[] ConfigLocations;

        public ConfigCreatorService()
        {
            SharedContent.CurrentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            logPath = SharedContent.FilePathCreator(SharedContent.CurrentPath, SharedContent.LogPath);
            configPath = SharedContent.FilePathCreator(SharedContent.CurrentPath, SharedContent.ConfigDirectoryPath);

            this.ConfigLocations = new Tuple<string, string>[] {
                new Tuple<string, string> (logPath,   SharedContent.LogName),
                new Tuple<string, string> (configPath,  SharedContent.DirListFileLocation), // only need 
                new Tuple<string, string> (configPath,  SharedContent.ConfigFullPath )
            };
        }

        public virtual bool CheckConfigExists()
        {
            return Directory.Exists(SharedContent.ConfigDirectoryPath);
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
                                foreach (var config in baseConfigValues)
                                {
                                    // Create config file with boilerplate values
                                    {
                                        sw.WriteLine(config);
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
