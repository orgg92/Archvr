namespace archiver.Application.Tests.HandlerTests
{
    using archiver.Application.Handlers.ConfigLoader;
    using archiver.Core;
    using FluentAssertions;
    using NSubstitute;
    using System.Threading.Tasks;

    [TestClass]
    [TestCategory(TEST_DESCRIPTION)]
    public class ConfigLoaderTests : TestBase
    {
        public const string TEST_DESCRIPTION = "ConfigLoader Handler Tests";

        private ConfigLoaderCommand _request;

        private ConfigLoaderCommand BuildRequest()
        {
            return new ConfigLoaderCommand();
        }

        [TestInitialize]
        public async Task Initialize()
        {
            await base.Initialize();
            _request = BuildRequest();
        }

        [TestMethod]
        public async Task IfConfigExists_LoadConfig_ShouldBeSuccess()
        {
            _configCreatorService.CheckConfigExists().Returns(true);
            _configCreatorService.CheckConfigHasBeenTouched().Returns(true);      

            var result = await SendAsync(_request);

            _configCreatorService.Received().LoadConfig();
            result.ConfigLoaded.Should().BeTrue();
        }

        [TestMethod]
        public async Task IfConfigNotExists_LoadConfig_ShouldBeFalse()
        {
            _configCreatorService.CheckConfigExists().Returns(false);
            _configCreatorService.CheckConfigHasBeenTouched().Returns(false);

            var result = await SendAsync(_request);

            _configCreatorService.DidNotReceive().LoadConfig();
            result.ConfigLoaded.Should().BeFalse();
        }

        [TestMethod]
        public async Task IfConfigNotTouched_LoadConfig_ShouldBeFalse()
        {
            _configCreatorService.CheckConfigExists().Returns(true);
            _configCreatorService.CheckConfigHasBeenTouched().Returns(false);

            var result = await SendAsync(_request);

            _configCreatorService.DidNotReceive().LoadConfig();
            result.ConfigLoaded.Should().BeFalse();
        }
    }
}
