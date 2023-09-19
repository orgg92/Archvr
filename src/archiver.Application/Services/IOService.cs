namespace archiver.Application.Services
{
    using archiver.Application.Interfaces;
    using archiver.Core;
    using System.Collections.Generic;
    using System.Linq;

    public class IOService : IIOService
    {
        public IEnumerable<string> ReadConfigFileDirectoryList()
        {
            // read directories from DirListFileLocation [i.e. the text file of directories to archive]

            return File.ReadAllLines(ProgramConfig.DirListFileLocation);
        }

        public string[] ReturnFileList(string directory)
        {
            return Directory.EnumerateFiles(ProgramConfig.FilePathCreator(ProgramConfig.FormatDriveToStringContext(), directory)).ToArray();
        }

        public bool CheckIfFileShouldBeUpdated(string srcPath, string destPath)
        {
            return CheckFileExists(srcPath) && CheckIfSrcFileIsNewerThanDestFile(srcPath, destPath);
        }

        public bool CheckDirectoryExists(string directory)
        {
            return Directory.Exists(directory);
        }

        public bool CheckFileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public bool CheckIfSrcFileIsNewerThanDestFile(string srcPath, string destPath)
        {
            return (File.GetLastWriteTimeUtc(srcPath) > File.GetLastWriteTimeUtc(destPath));
        }

        public void CreateDirectory(string directory)
        {
            Directory.CreateDirectory(directory);
        }

        public void CopyFile(string srcPath, string destPath)
        {
            if (CheckFileExists(destPath))
            {
                File.Delete(destPath);
            }

            File.Copy(srcPath, destPath);
        }

        public string GetDestinationDirectory(string fileName)
        {
            return ProgramConfig.ReplaceDriveToArchiveContext(ProgramConfig.DestinationDrive.ToCharArray()[0], new DirectoryInfo(fileName).Parent.ToString());
        }


    }
}
