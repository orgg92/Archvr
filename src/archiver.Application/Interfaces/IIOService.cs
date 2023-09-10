namespace archiver.Application.Interfaces
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
        List<string> ReturnFileList();
        bool CheckIfFileShouldBeUpdated(string srcPath, string destPath);
        string GetDestinationDirectory(string fileName);

    }
}