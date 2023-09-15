namespace archiver.Application.Tests.HandlerTests
{
    using archiver.Application.Handlers.FolderScanner;
    using FluentAssertions;
    using NSubstitute;
    using NSubstitute.ReceivedExtensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    [TestCategory(TEST_DESCRIPTION)]
    public class FolderScannerTests : TestBase
    {
        public const string TEST_DESCRIPTION = "FolderScanner Handler Tests";

        private FolderScannerCommand _request;

        private FolderScannerCommand BuildRequest()
        {
            return new FolderScannerCommand();
        }

        [TestInitialize]
        public async Task Initialize()
        {
            await base.Initialize();
            _request = BuildRequest();
        }

        [TestMethod]
        public async Task IOService_Returns_ListOfFiles()
        {
            var fileList = new string[] { "test.txt", "test.log" }.ToArray();
            var resultList = new string[] { "test.txt", "test.log", "test.txt", "test.log" }.ToArray();
            var dirList = new string[] { "Y:\\", "Z:\\" }.ToArray();

            _ioService.CheckDirectoryExists(Arg.Any<string>()).Returns(true);
            _ioService.ReadConfigFileDirectoryList().Returns(dirList);
            _ioService.ReturnFileList(Arg.Any<string>()).Returns(fileList);
            var result = await SendAsync(_request);

            _ioService.Received(1).ReadConfigFileDirectoryList();
            //_ioService.Received(2).CheckDirectoryExists(Arg.Any<string>());
            _ioService.Received(2).ReturnFileList(Arg.Any<string>());

            result.FileList.Should().Equal(resultList);
        }
    }
}
