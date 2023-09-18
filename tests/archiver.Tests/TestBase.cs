namespace archiver.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using System.Threading.Tasks;
    using static archiver.Tests.TestRoot;

    public abstract class TestBase
    {
        [TestInitialize]
        public virtual async Task Initialize()
        {
            // needed or running all tests together start failing
            _configService.ClearReceivedCalls();
            _ioService.ClearReceivedCalls();
            _consoleService.ClearReceivedCalls();
        }

    }
}
