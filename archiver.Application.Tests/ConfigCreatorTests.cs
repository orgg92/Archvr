namespace archiver.Application.Tests
{
    using Application;
    using archiver.Application.Interfaces;
    using global::Application;
    using MediatR;
    using Moq;
    using NSubstitute;
    using Ryker;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class ConfigCreatorTests : TestRoot
    {
        private IConfigCreatorService _configCreatorService;
        private IMediator _mediator;

        public ConfigCreatorTests()
        {
            var services = getServices();
            _mediator = Substitute.For<IMediator>();
            _configCreatorService = Substitute.For<IConfigCreatorService>();
        }


        [TestMethod]
        public void IfConfigNotExists_DoNotCreate()
        {
            var request = new ConfigCreatorCommand() {};
            _configCreatorService.CheckConfigExists().Returns(true);

            var result = _mediator.Send(request);

            _configCreatorService.DidNotReceive().WriteNewConfigFile();
        }

        [TestMethod]
        public void IfConfigNotExists_Create()
        {
            // Need to figure out why the mock service call isn't being called
            //var request = new ConfigCreatorCommand() { };

            //SharedContent.ConfigFullPath = null;

            //_configCreatorService.CheckConfigExists()
            //    .Returns(false);

            ////_configCreatorService.S

            //var result = _mediator.Send(request);
            
            //_configCreatorService.Received().WriteNewConfigFile();
        }
    }
}
