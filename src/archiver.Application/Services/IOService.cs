namespace archiver.Application.Services
{
    using archiver.Application.Interfaces;
    using archiver.Core;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class IOService : IIOService
    {
        public IEnumerable<string> ReadConfigFileDirectoryList()
        {
            // read directories from DirListFileLocation [i.e. the text file of directories to archive]

            return File.ReadAllLines(ProgramConfig.DirListFileLocation);
        }

        public List<string> ReturnFileList()
        {
            var folderList = ReadConfigFileDirectoryList();
            var fileList = new List<string>();

            foreach (var directory in folderList)
            {
                fileList.AddRange(Directory.EnumerateFiles(ProgramConfig.FilePathCreator(ProgramConfig.FormatDriveToStringContext(), directory)));
            }

            return fileList;
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
