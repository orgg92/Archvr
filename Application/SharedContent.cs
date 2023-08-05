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
             LogPath = $"{CurrentPath}\\Logs\\",
             LogName = $"{LogPath}\\logfile.log",
             ConfigDirectoryPath = $"{CurrentPath}\\Config\\",
             ConfigFullPath = $"{ConfigDirectoryPath}\\config.ini",
             DirListFileLocation = $"{ConfigDirectoryPath}\\directory-list.txt",
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

        public static IEnumerable<ProgramException> ProgramExceptions = new ProgramException[]
        {
            new ProgramException() { ErrorCode = "CONFIG_PARSE", ErrorMessage = "There was an issue loading config" },
            new ProgramException() { ErrorCode = "FILE_LOCK", ErrorMessage = "A file was locked for too long" }

        };

        public static IEnumerable<HandlerLoggingKeyValuePair> HandlerLoggingMessages = new HandlerLoggingKeyValuePair[]
        {
            new HandlerLoggingKeyValuePair() { Name = "ConfigCreatorCommand", Message = "Checking for config..."},
            new HandlerLoggingKeyValuePair() { Name = "ConfigLoaderCommand", Message = "Attempting to load config..."},
            new HandlerLoggingKeyValuePair() { Name = "FolderScannerCommand", Message = "Starting to scan directories..."},
            new HandlerLoggingKeyValuePair() { Name = "FileArchiverCommand", Message = "Attempting to archive file..."}
        };





    }

    public class HandlerLoggingKeyValuePair
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }


    public class ProgramException
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

}
