namespace Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class SharedContent
    {
        public static string[] configValues = new string[10];

        public static bool LogProgressToConsole;

        public static string FilePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
             CurrentPath = AppDomain.CurrentDomain.BaseDirectory,
             TargetDrive = $"{configValues[0]}",
             DestinationDrive = String.Empty,
             Spacer = "************************************************************** \r\n",
             WelcomeMessage = $"{Spacer} [{DateTime.UtcNow}]: Starting archive process \r\n{Spacer}",
             LogPath = FilePathCreator(CurrentPath, "Logs\\"),
             LogName = FilePathCreator(LogPath, "logfile.log"),
             ConfigDirectoryPath = FilePathCreator(CurrentPath, "Config\\"),
             ConfigFullPath = FilePathCreator(ConfigDirectoryPath, "config.ini"),
             DirListFileLocation = FilePathCreator(ConfigDirectoryPath, "directory-list.txt"),
             UserDestinationDrive = String.Empty,
             OutputLocation = String.Empty,
             RetryCount = String.Empty,
             ProgressMeter = $"{0}% complete",
             ResponsiveSpacer = String.Empty;

        public static int ConsoleHeight, ConsoleWidth;

        public static string logo = @"   
                _            _     _                
               /_\  _ __ ___| |__ (_)_   _____ _ __ 
              //_\\| '__/ __| '_ \| \ \ / / _ \ '__|
             /  _  \ | | (__| | | | |\ V /  __/ |   
             \_/ \_/_|  \___|_| |_|_| \_/ \___|_|   
                                                   ";

        public static string[] UserGuideMessages =
        {
            "Entering user guided mode...",
            $"What is the drive letter of the destination drive? (Leave blank for default: {SharedContent.DestinationDrive})",
            $"What is the file path to the location of file containing files for backup? (Leave blank for default: {SharedContent.DirListFileLocation})"
        };

        public static IEnumerable<HandlerLoggingKeyValuePair> ProgramExceptions = new HandlerLoggingKeyValuePair[]
        {
            new HandlerLoggingKeyValuePair() { Name = "CONFIG_PARSE", Message = "There was an issue loading config." },
            new HandlerLoggingKeyValuePair() { Name = "FILE_LOCK", Message = "A file was locked for too long." },
            new HandlerLoggingKeyValuePair() { Name = "CONFIG_CREATION_ERROR", Message = "There was an issue creating program config."},
            new HandlerLoggingKeyValuePair() { Name = "CONFIG_LOAD_ERROR", Message = "There was an issue parsing program config."},
            new HandlerLoggingKeyValuePair() { Name = "FOLDER_SCAN", Message = "There was an issue scanning directories."}

        };

        public static IEnumerable<HandlerLoggingKeyValuePair> HandlerLoggingMessages = new HandlerLoggingKeyValuePair[]
        {
            new HandlerLoggingKeyValuePair() { Name = "ConfigCreatorCommand", Message = "Checking for config..."},
            new HandlerLoggingKeyValuePair() { Name = "ConfigLoaderCommand", Message = "Attempting to load config..."},
            new HandlerLoggingKeyValuePair() { Name = "FolderScannerCommand", Message = "Starting to scan directories..."},
            new HandlerLoggingKeyValuePair() { Name = "FileArchiverCommand", Message = "Attempting to archive file..."}
        };

        public static string FilePathCreator(string directory, string filePath)
        {
            return System.IO.Path.Combine(directory, filePath).Replace(@"\\", @"\");
        }

        public static string FormatDriveToStringContext()
        {
            return $"{SharedContent.TargetDrive}:\\";
        }

    }

    public class HandlerLoggingKeyValuePair
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }


    public class ProgramException : Exception
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

}
