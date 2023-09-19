namespace archiver.Tests
{
    using archiver.Application.Handlers.ConfigCreator;
    using archiver.Application.Handlers.ConfigLoader;
    using archiver.Application.Handlers.FileArchiver;
    using archiver.Application.Handlers.FolderScanner;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using static archiver.Tests.TestRoot;

    /// <summary>
    /// Tests of the main class
    /// </summary>

    [TestClass]
    [TestCategory("Program Tests")]
    public class ProgramTests : TestBase
    {
        private List<string> fileList;
        private Archiver _archiver;

        [TestInitialize]
        public async Task Initialize()
        {
            base.Initialize();

            fileList = new List<string>()
            {
                "file1.txt",
                "file.log"
            };

            _archiver = Substitute.For<Archiver>(_mediator, _consoleService);
        }


        /// <summary>
        /// Failed config creation means the program can't continue
        /// </summary>
        /// 

        [TestMethod]
        public async Task IfConfigCreationFails_ShouldNotRunToCompletion()
        {
            _archiver.CreateConfig().Returns(new ConfigCreatorResponse() { ConfigCreated = ConfigCreated.True });

            await _archiver.Initialize();

            await _archiver.Received().CreateConfig();
            await _archiver.DidNotReceive().LoadConfig();
            await _archiver.DidNotReceive().ScanDirectories();
            await _archiver.DidNotReceive().ArchiveFile(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>());

        }

        /// <summary>
        /// Config creation should not proceed further than the config creation step
        /// </summary>
        /// 

        [TestMethod]
        public async Task IfConfigCreationSucceeds_ShouldRunToCompletion()
        {
            _archiver.CreateConfig().Returns(new ConfigCreatorResponse() { ConfigCreated = ConfigCreated.False });
            _archiver.LoadConfig().Returns(new ConfigLoaderResponse() { ConfigLoaded = true, HandlerException = null });
            _archiver.ScanDirectories().Returns(new FolderScannerResponse() { FileList = fileList });
            _archiver.ArchiveFile(Arg.Any<string>()).Returns(new FileArchiverResponse() { ArchiveSuccess = true });
            _archiver.ArchiveFile(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new FileArchiverResponse() { ArchiveSuccess = true });

            await _archiver.Initialize();

            await _archiver.Received().CreateConfig();
            await _archiver.Received().LoadConfig();
            await _archiver.Received().ScanDirectories();
            await _archiver.Received().ArchiveFile(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>());

        }

    }
}
