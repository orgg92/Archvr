namespace archiver.Tests
{
    using archiver.Application.Handlers.ConfigCreator;
    using archiver.Application.Handlers.ConfigLoader;
    using archiver.Application.Handlers.FileArchiver;
    using archiver.Application.Handlers.FolderScanner;
    using archiver.Core;
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
            await _archiver.DidNotReceive().ScanConfigForDirectories();
            await _archiver.DidNotReceive().ArchiveFile(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>());

        }

        /// <summary>
        /// Failed config load means the program can't continue
        /// </summary>
        /// 

        [TestMethod]
        public async Task IfConfigLoadingFails_ShouldNotRunToCompletion()
        {
            _archiver.CreateConfig().Returns(new ConfigCreatorResponse() { ConfigCreated = ConfigCreated.False });
            _archiver.LoadConfig().Returns(new ConfigLoaderResponse() { ConfigLoaded = false, HandlerException = null });

            await _archiver.Initialize();

            await _archiver.Received().CreateConfig();
            await _archiver.Received().LoadConfig();
            await _archiver.DidNotReceive().ScanConfigForDirectories();
            await _archiver.DidNotReceive().ArchiveFile(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>());

        }

        /// <summary>
        /// Program should run to completion
        /// </summary>
        /// 

        [TestMethod]
        public async Task IfConfigCreationSucceeds_ShouldRunToCompletion()
        {
            _archiver.CreateConfig().Returns(new ConfigCreatorResponse() { ConfigCreated = ConfigCreated.False });
            _archiver.LoadConfig().Returns(new ConfigLoaderResponse() { ConfigLoaded = true, HandlerException = null });
            //_archiver.ScanConfigForDirectories().Returns(new FolderScannerResponse() { FileList = fileList });
            _archiver.ArchiveFile(Arg.Any<string>()).Returns(new FileArchiverResponse() { ArchiveSuccess = true });
            _archiver.ArchiveFile(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new FileArchiverResponse() { ArchiveSuccess = true });

            await _archiver.Initialize();

            await _archiver.Received().CreateConfig();
            await _archiver.Received().LoadConfig();
            await _archiver.Received().ScanConfigForDirectories();
            await _archiver.Received(2).ArchiveFile(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>());

        }

        /// <summary>
        /// Locked files should be handled if present
        /// </summary>
        /// 

        [TestMethod]
        public async Task IfAnyLockedFiles_ShouldBeArchived()
        {
            _archiver.CreateConfig().Returns(new ConfigCreatorResponse() { ConfigCreated = ConfigCreated.False });
            _archiver.LoadConfig().Returns(new ConfigLoaderResponse() { ConfigLoaded = true, HandlerException = null });
            _archiver.ScanConfigForDirectories().Returns(new FolderScannerResponse() { FileList = fileList });
            _archiver.ArchiveFile(Arg.Any<string>()).Returns(new FileArchiverResponse() { ArchiveSuccess = false });
            _archiver.ArchiveFile(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new FileArchiverResponse() { ArchiveSuccess = false });
            ProgramConfig.RetryCount = 5;

            await _archiver.Initialize();

            await _archiver.Received().CreateConfig();
            await _archiver.Received().LoadConfig();
            await _archiver.Received().ScanConfigForDirectories();
            await _archiver.Received(4).ArchiveFile(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>());



        }

    }
}
