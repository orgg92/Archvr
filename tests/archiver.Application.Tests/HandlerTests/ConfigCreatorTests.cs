namespace archiver.Application.Tests.HandlerTests
{
    using archiver.Application.Handlers.ConfigCreator;
    using NSubstitute;

    [TestClass]
    [TestCategory(TEST_DESCRIPTION)]
    public class ConfigCreatorTests : TestBase
    {
        public const string TEST_DESCRIPTION = "ConfigCreator Handler Tests";

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
            _configService.CheckConfigExists().Returns(true);

            await SendAsync(_request);

            _configService.DidNotReceive().WriteNewConfigFile();
        }

        [TestMethod]
        public async Task IfConfigNotExists_Create()
        {
            _configService.CheckConfigExists().Returns(false);

            await SendAsync(_request);

            _configService.Received().WriteNewConfigFile();

        }
    }
}
