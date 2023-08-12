namespace Application
{
    using MediatR.Wrappers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    // <summary>
    // a collection of resources and methods that can be used throughout the program
    // </summary>

    public static class SharedContent
    {
        public static IEnumerable<ConfigSetting> configValues;

        public static bool LogProgressToConsole;

        public static string FilePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
             CurrentPath = AppDomain.CurrentDomain.BaseDirectory,
             TargetDrive = null,
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
             ResponsiveSpacer = String.Empty,
             ArchiveFolderName = String.Empty,
             ArchivePath = String.Empty; // this is {targetDrive}/{archiveFolderName}/< full file path >

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
            new HandlerLoggingKeyValuePair() { Name = "FOLDER_SCAN", Message = "There was an issue find/scanning the source directories."}

        };

        public static IEnumerable<HandlerLoggingKeyValuePair> HandlerLoggingMessages = new HandlerLoggingKeyValuePair[]
        {
            new HandlerLoggingKeyValuePair() { Name = "ConfigCreatorCommand", Message = "Checking for config..."},
            new HandlerLoggingKeyValuePair() { Name = "ConfigLoaderCommand", Message = "Attempting to load config..."},
            new HandlerLoggingKeyValuePair() { Name = "FolderScannerCommand", Message = "Starting to scan directories..."},
            new HandlerLoggingKeyValuePair() { Name = "FileArchiverCommand", Message = "Attempting to archive file..."}
        };

        public static IEnumerable<HandlerLoggingKeyValuePair> HandlerErrorMessages = new HandlerLoggingKeyValuePair[]
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

        public static string ReplaceDriveToArchiveContext(char drive, string filepath)
        {
            var directoryName = new DirectoryInfo(filepath).Name;
            var archiveContextPath = $"{drive}:\\{SharedContent.ArchiveFolderName}\\{directoryName}";
            return new string(archiveContextPath);
        }

        public static string GetFullArchiveAndFilePath(string filepath)
        {
            var fileName = Path.GetFileName(filepath);
            var drive = SharedContent.DestinationDrive;
            var archiveFolder = SharedContent.ArchiveFolderName;
            var archivePath = System.IO.Path.Combine($"{drive}:\\{archiveFolder}", fileName).Replace(@"\\", @"\");
            return System.IO.Path.Combine($"{drive}:\\{archiveFolder}", fileName).Replace(@"\\", @"\");
        }

        // returns error message for the specified error code
        public static string ReturnErrorMessageForErrorCode(string errorCode)
        {
            return SharedContent.ProgramExceptions.Where(y => y.Name == errorCode).Select(y => y.Message).First();
        }

        // returns the message to be logged to the console corresponding to the appropriate handler 
        public static string ReturnMessageForHandler(string handlerName)
        {
            return SharedContent.HandlerLoggingMessages.Where(y => y.Name == handlerName).Select(y => y.Message).First();
        }

        public static void LogToConsole(string message, bool? includeEndingSpacer = false)
        {
            Console.WriteLine(ResponsiveSpacer);
            Console.WriteLine(message);
            if (includeEndingSpacer != null || includeEndingSpacer is not false)
            {
                Console.WriteLine(ResponsiveSpacer);
            }
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

    public class ConfigSetting
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }


}
