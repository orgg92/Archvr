namespace archiver.Core
{
    using System;
    using System.Collections.Generic;

    public static class ProgramConfig
    {
        public static IEnumerable<WriteConfigSettingModel> configValues;

        /// <summary>
        /// Variables imported from text file for use throughout the application
        /// </summary>

        public static bool LogProgressToConsole;

        public static int
            ConsoleHeight,
            ConsoleWidth,
            RetryCount,
            LogLevel
           ;


        public static string FilePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
             CurrentPath = AppDomain.CurrentDomain.BaseDirectory,
             TargetDrive = String.Empty,
             DestinationDrive = String.Empty,
             LogPath = FilePathCreator(CurrentPath, "Logs\\"),
             LogName = FilePathCreator(LogPath, "logfile.log"),
             ConfigDirectoryPath = FilePathCreator(CurrentPath, "Config\\"),
             ConfigFullPath = FilePathCreator(ConfigDirectoryPath, "config.ini"),
             DirListFileLocation = FilePathCreator(ConfigDirectoryPath, "directory-list.txt"),
             OutputLocation = String.Empty,
             ResponsiveSpacer = String.Empty,
             ArchiveFolderName = String.Empty,
             ArchivePath = String.Empty // this is {targetDrive}/{archiveFolderName}/< full file path >
            ;

        public static string FilePathCreator(string directory, string filePath)
        {
            return Path.Combine(directory, filePath).Replace(@"\\", @"\");
        }

        public static string FormatDriveToStringContext()
        {
            return $"{ProgramConfig.TargetDrive}:\\";
        }

        public static string ReplaceDriveToArchiveContext(char drive, string filepath)
        {
            var directoryName = new DirectoryInfo(filepath).Name;
            var archiveContextPath = $"{drive}:\\{ProgramConfig.ArchiveFolderName}\\{directoryName}";
            return new string(archiveContextPath);
        }

        public static string GetFullArchiveAndFilePath(string filepath)
        {
            var fileName = Path.GetFileName(filepath);
            var folderName = Path.GetDirectoryName(filepath);
            var drive = ProgramConfig.DestinationDrive;
            var archiveFolder = ProgramConfig.ArchiveFolderName;
            return Path.Combine($"{drive}:\\{archiveFolder}", fileName).Replace(@"\\", @"\");
        }
    }

    public class BaseConfigKeyValuePair
    {
        private string Name { get; }
        private string Value { get; }

        public BaseConfigKeyValuePair(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Creates a config string in config file format, ie. NAME='VALUE'
        /// </summary>
        public string ReturnConfigReadyString()
        {
            return $"{Name}='{Value}'";
        }
    }

    public class BaseConfigValues
    {
        public IEnumerable<BaseConfigKeyValuePair> Values { get; set; }

        public string[] baseConfigWritePropertyNames = new string[]
        {
            "TARGET_DRIVE",
            "DESTINATION_DRIVE",
            "RETRY_COUNT",
            "LOG_PROGRESS_TO_CONSOLE",
            "DIRFILE_LOCATION",
            "OUTPUT_LOCATION",
            "CONSOLE_HEIGHT",
            "CONSOLE_WIDTH",
            "ARCHIVE_FOLDER_NAME",
            "LOG_LEVEL"
        };

        public string[] baseConfigWritePropertyValues = new string[]
        {
            String.Empty,
            String.Empty,
            "1",
            "true",
            ProgramConfig.FilePathCreator(ProgramConfig.ConfigDirectoryPath, "directory-list.txt"),
            String.Empty,
            "25",
            "100",
            "Archive",
            "0" // 0 = basic handler messages, 1 = file status info, 2 = verbose logging
        };

        public string[] baseConfigReadValueTypes = new string[]
        {
            typeof(string).Name,
            typeof(string).Name,
            typeof(int).Name,
            typeof(bool).Name,
            typeof(string).Name,
            typeof(string).Name,
            typeof(int).Name,
            typeof(int).Name,
            typeof(string).Name,
            typeof(int).Name,
        };

        public BaseConfigValues()
        {
            var configValues = new List<BaseConfigKeyValuePair>();

            for (int i = 0; i < baseConfigWritePropertyNames.Length; i++)
            {
                configValues.Add(new BaseConfigKeyValuePair(baseConfigWritePropertyNames[i], baseConfigWritePropertyValues[i]));
            }

            Values = configValues;
        }
    }

}