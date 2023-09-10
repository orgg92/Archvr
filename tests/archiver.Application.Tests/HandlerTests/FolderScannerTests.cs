namespace archiver.Application.Tests.HandlerTests
{
    using archiver.Application.Handlers.FolderScanner;
    using FluentAssertions;
    using NSubstitute;
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
            var fileList = new string[] { "test.txt", "test.log" }.ToList();

            _ioService.ReturnFileList().Returns(fileList);
            var result = await SendAsync(_request);

            _ioService.Received(1).ReturnFileList();

            result.FileList.Should().Equal(fileList);
        }
    }
}
