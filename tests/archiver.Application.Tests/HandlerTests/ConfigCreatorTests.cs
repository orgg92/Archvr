namespace archiver.Application.Tests.HandlerTests
{
    using archiver.Application.Handlers.ConfigCreator;
    using NSubstitute;

    [TestClass]
    public class ConfigCreatorTests : TestBase
    {
        private ConfigCreatorCommand _request;

        public ConfigCreatorTests()
        {

        }

        private ConfigCreatorCommand BuildRequest()
        {
            return new ConfigCreatorCommand();
        }

        [TestInitialize]
        public async Task Initialize()
        {
            await base.Initialize();
            _request = BuildRequest();

        }

        [TestMethod]
        public async Task IfConfigExists_DoNotCreate()
        {
            _configCreatorService.CheckConfigExists().Returns(true);

            await SendAsync(_request);

            _configCreatorService.DidNotReceive().WriteNewConfigFile();
        }

        [TestMethod]
        public async Task IfConfigNotExists_Create()
        {
            _configCreatorService.CheckConfigExists().Returns(false);

            await SendAsync(_request);

            _configCreatorService.Received().WriteNewConfigFile();

        }
    }
}
