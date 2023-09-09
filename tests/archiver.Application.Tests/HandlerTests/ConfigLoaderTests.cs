namespace archiver.Application.Tests.HandlerTests
{
    using archiver.Application.Handlers.ConfigLoader;
    using System.Threading.Tasks;

    public class ConfigLoaderTests : TestBase
    {
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
        public async Task IfConfigNotExists_Throw()
        {
            var result = await SendAsync(_request);


        }

        [TestMethod]
        public async Task IfConfigNotTouched_Throw()
        {

        }

        // if pre requisite conditions met

    }
}
