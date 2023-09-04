namespace archiver.Application.Tests
{
    using Moq;

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
            
            _configCreatorService.Setup(x => x.CheckConfigExists()).Returns(true);

            await SendAsync(_request);

            _configCreatorService.Verify(x => x.WriteNewConfigFile(), Times.Never);
        }

        [TestMethod]
        public async Task IfConfigNotExists_Create()
        {

            _configCreatorService.Setup(x => x.CheckConfigExists()).Returns(false);

            await SendAsync(_request);

            _configCreatorService.Verify(x => x.WriteNewConfigFile(), Times.Once);

        }
    }
}
