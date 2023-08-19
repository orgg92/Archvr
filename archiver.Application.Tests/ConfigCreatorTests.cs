namespace archiver.Application.Tests
{
    using Application.Interfaces;
    using global::Application.Handlers.ConfigCreator;
    using global::Application.Interfaces;
    using NSubstitute;
    using Ryker;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class ConfigCreatorTests : TestRoot
    {
        private IConsoleService _consoleService;
        private ILoggerService _loggerService;


        [TestInitialize]
        public void Initialize()
        {
            _consoleService = Substitute.For<IConsoleService>();
            _loggerService = Substitute.For<ILoggerService>();

            
        }
    }
}
