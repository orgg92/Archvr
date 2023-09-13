namespace archiver.Tests
{
    using archiver.Application.Handlers.ConfigCreator;
    using archiver.Application.Handlers.ConfigLoader;
    using archiver.Application.Handlers.FileArchiver;
    using archiver.Application.Handlers.FolderScanner;
    using archiver.Application.Interfaces;
    using archiver.Infrastructure.Interfaces;
    using MediatR;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using System.Collections;

    [TestClass]
    [TestCategory("Program Tests")]
    public class ProgramTests
    {
        private IMediator _mockMediator;
        private IConsoleService _mockConsoleService;
        private Program _program;

        [TestInitialize]
        public async Task Initialize()
        {
            _mockMediator = Substitute.For<IMediator>();
            _mockConsoleService = Substitute.For<IConsoleService>();
            _program = new Program(_mockMediator, _mockConsoleService);
        }

        [TestMethod]
        public async Task WhenAllTrue_FollowHappyPath()
        {
            var testFileName = "test_file.txt";
            var fileList = new List<string>()
            {
                testFileName
            };

            _mockMediator.When(x => x.Send(Arg.Any<ConfigCreatorCommand>()).Returns(new ConfigCreatorResponse()
            {
                ConfigCreated = ConfigCreated.True,
                HandlerException = null
            }));

            _mockMediator.When(x => x.Send(Arg.Any<ConfigLoaderCommand>()).Returns(new ConfigLoaderResponse()
            {
                ConfigLoaded = true,
                HandlerException = null
            }));

            _mockMediator.When(x => x.Send(Arg.Any<FolderScannerCommand>()).Returns(new FolderScannerResponse()
            {
                FileList = fileList,
                HandlerException = null
            })) ;

            _mockMediator.When(x => x.Send(Arg.Any<FileArchiverCommand>()).Returns(new FileArchiverResponse()
            {
               ArchiveSuccess = true,
               HandlerException = null
            }));

        }
    }
}
