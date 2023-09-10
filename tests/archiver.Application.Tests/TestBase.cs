namespace archiver.Application.Tests
{
    using NSubstitute;
    using System.Threading.Tasks;

    public abstract class TestBase
    {
        [TestInitialize]
        public virtual async Task Initialize()
        {
            // needed or running all tests together start failing
            _configCreatorService.ClearReceivedCalls();
            _ioService.ClearReceivedCalls();
        }
    }
}
