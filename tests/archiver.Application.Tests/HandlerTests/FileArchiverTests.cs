namespace archiver.Application.Tests.HandlerTests
{
    using archiver.Application.Handlers.FileArchiver;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    [TestCategory(TEST_DESCRIPTION)]
    public class FileArchiverTests : TestBase
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
            await base.Initialize();
            _request = BuildRequest();
        }

        [TestMethod]
        public async Task IfDirectoryNotExists_Create()
        {
            _ioService.CheckDirectoryExists(Arg.Any<string>()).Returns(false);

            var result = await SendAsync(_request);

            _ioService.Received(1).CheckDirectoryExists(Arg.Any<string>());
            _ioService.Received(1).CreateDirectory(Arg.Any<string>());
        }

        [TestMethod]
        public async Task IfFileNotExistsOrNewer_CopyFile()
        {
            _ioService.CheckDirectoryExists(Arg.Any<string>()).Returns(true);
            _ioService.CheckIfFileShouldBeUpdated(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

            var result = await SendAsync(_request);

            _ioService.Received(1).CheckDirectoryExists(Arg.Any<string>());
            _ioService.Received(0).CreateDirectory(Arg.Any<string>());

        }

        [TestMethod]
        public async Task IfFileUpToDate_SkipFile()
        {
            _ioService.CheckDirectoryExists(Arg.Any<string>()).Returns(true);
            _ioService.CheckIfFileShouldBeUpdated(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

            var result = await SendAsync(_request);

            _ioService.Received(1).CheckDirectoryExists(Arg.Any<string>());
            _ioService.Received(0).CreateDirectory(Arg.Any<string>());
            _ioService.Received(0).CopyFile(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
