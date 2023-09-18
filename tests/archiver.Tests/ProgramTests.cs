namespace archiver.Tests
{
    using archiver.Application.Interfaces;
    using archiver.Infrastructure.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static archiver.Tests.TestRoot;
    using NSubstitute;
    using archiver.Application.Handlers.ConfigCreator;
    using archiver.Application.Handlers.ConfigLoader;

    /// <summary>
    /// Tests of the main class
    /// </summary>

    [TestClass]
    [TestCategory("Program Tests")]
    public class ProgramTests : TestBase
    {
        [TestInitialize]
        public async Task Initialize()
        {
            base.Initialize();            
        }


        /// <summary>
        /// Failed config creation means the program can't continue
        /// </summary>
        [TestMethod]
        public async Task IfConfigCreationFails_ShouldNotContinue()
        {
            _archiver.CreateConfig().Returns(new ConfigCreatorResponse() { ConfigCreated = ConfigCreated.False });


            await _archiver.Initialize();


            await _archiver.Received().LoadConfig();
            await _archiver.Received().ScanDirectories();
            await _archiver.Received().ProcessFileList(Arg.Any<string[]>(), Arg.Any<bool>());

        }

    }
}
