namespace archiver.Core
{
    using archiver.Core.Enum;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // <summary>
    // a collection of static content, resources and methods that can be used throughout the program
    // </summary>

    public static class SharedContent
    {

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
            $"What is the drive letter of the destination drive? (Leave blank for default: {ProgramConfig.DestinationDrive})",
            $"What is the file path to the location of file containing files for backup? (Leave blank for default: {ProgramConfig.DirListFileLocation})"
        };

        public static IEnumerable<HandlerLoggingKeyValuePair> ProgramExceptions = new HandlerLoggingKeyValuePair[]
        {
            new HandlerLoggingKeyValuePair() { Key = "CONFIG_PARSE", Value = "There was an issue loading config." },
            new HandlerLoggingKeyValuePair() { Key = "FILE_LOCK", Value = "A file was locked for too long." },
            new HandlerLoggingKeyValuePair() { Key = "ARCHIVE_ERROR", Value = "An error occurred while trying to archive file: {0}" },
            new HandlerLoggingKeyValuePair() { Key = "CONFIG_CREATION_ERROR", Value = "There was an issue creating program config."},
            new HandlerLoggingKeyValuePair() { Key = "CONFIG_LOAD_ERROR", Value = "There was an issue parsing program config."},
            new HandlerLoggingKeyValuePair() { Key = "FOLDER_SCAN", Value = "There was an issue find/scanning the source directories."}

        };

        public static IEnumerable<HandlerLoggingKeyValuePair> HandlerLoggingMessages = new HandlerLoggingKeyValuePair[]
        {
            new HandlerLoggingKeyValuePair() { Key = "ConfigCreatorCommand", Value = "Checking for config..."},
            new HandlerLoggingKeyValuePair() { Key = "ConfigLoaderCommand", Value = "Attempting to load config..."},
            new HandlerLoggingKeyValuePair() { Key = "FolderScannerCommand", Value = "Starting to scan directories..."},
            new HandlerLoggingKeyValuePair() { Key = "FileScannerCommand", Value = "Attempting to scan directory for files..."},
            new HandlerLoggingKeyValuePair() { Key = "FileArchiverCommand", Value = "Attempting to archive files..."}
        };

        public static IEnumerable<HandlerLoggingKeyValuePair> HandlerErrorMessages = new HandlerLoggingKeyValuePair[]
        {
            new HandlerLoggingKeyValuePair() { Key = "ConfigCreatorCommand", Value = "Checking for config..."},
            new HandlerLoggingKeyValuePair() { Key = "ConfigLoaderCommand", Value = "Attempting to load config..."},
            new HandlerLoggingKeyValuePair() { Key = "FolderScannerCommand", Value = "Starting to scan directories..."},
            new HandlerLoggingKeyValuePair() { Key = "FileArchiverCommand", Value = "An error occurred while trying to archive file: {0}"}
        };


        // returns error message for the specified error code
        public static string ReturnErrorMessageForErrorCode(string errorCode)
        {
            return SharedContent.ProgramExceptions.Where(y => y.Key == errorCode).Select(y => y.Value).First();
        }

        // returns the message to be logged to the console corresponding to the appropriate handler 
        public static string ReturnMessageForHandler(string handlerName)
        {
            return SharedContent.HandlerLoggingMessages.Where(y => y.Key == handlerName).Select(y => y.Value).First();
        }

        public static string ReturnFormattedDateTimeToString()
        {
            return $"[{DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm")}]";
        }

        public static string ReturnDateFormattedConsoleMessage(string message)
        {
            return $"{ReturnFormattedDateTimeToString()} {message}";
        }
    }

    public class HandlerLoggingKeyValuePair
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class ProgramException : Exception
    {
        public ErrorCodes ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class WriteConfigSettingModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }


}
