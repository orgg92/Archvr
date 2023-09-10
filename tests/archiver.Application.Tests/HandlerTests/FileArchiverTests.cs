namespace archiver.Application.Tests.HandlerTests
{
    using archiver.Application.Handlers.FileArchiver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    [TestCategory(TEST_DESCRIPTION)]
    public class FileArchiverTests
    {
        public const string TEST_DESCRIPTION = "FileArchiver Handler Tests";

        public const string TEST_FILE_NAME = "Test_File.txt";

        public FileArchiverCommand _request;

        public FileArchiverCommand BuildRequest()
        {
            return new FileArchiverCommand()
            {
                FileName = TEST_FILE_NAME
            };
        }

        [TestInitialize]
        public async Task Initialize()
        {

        }
    }
}
