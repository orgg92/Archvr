namespace archiver.Core.Interfaces
{
    using System.Collections.Generic;

    public interface IIOService
    {
        bool CheckDirectoryExists(string directory);
        bool CheckFileExists(string filePath);
        bool CheckIfSrcFileIsNewerThanDestFile(string srcPath, string destPath);
        void CopyFile(string srcPath, string destPath);
        void CreateDirectory(string directory);
        IEnumerable<string> ReadConfigFileDirectoryList();
        string[] ReturnFileList(string directory);
        bool CheckIfFileShouldBeUpdated(string srcPath, string destPath);
        string GetDestinationDirectory(string fileName);

    }
}