namespace archiver.Application.Tests.HandlerTests
{
    using global::Application.Interfaces;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ConfigLoaderCommandTests : BaseTests
    {
        private IConsoleService _consoleService;

        public ConfigLoaderCommandTests(IConsoleService consoleService)
        {
            _consoleService = consoleService;

            _consoleService = Substitute.For<IConsoleService>();
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
             
        }

    }
}
