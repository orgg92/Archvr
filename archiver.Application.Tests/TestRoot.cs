namespace archiver.Application.Tests
{
    using archiver.Application.Interfaces;
    using global::Application;
    using global::Application.Interfaces;
    using global::Application.Services;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using NSubstitute;
    using NSubstitute.Extensions;
    using Ryker;

    [TestClass]
    public class TestRoot
    {

        public Startup startup;
        public IServiceProvider _services;

        public TestRoot()
        {
            //var config = new ConfigurationBuilder()
            //    .Build();

            //IServiceCollection services = new ServiceCollection();

            //startup = new Startup(config);
            //startup.ConfigureServices(services);

            //services.AddTransient<IConsoleService, ConsoleService>();

            //var realConfigService = services.FirstOrDefault(x => x.ServiceType == typeof(IConfigCreatorService));
            //services.Remove(realConfigService);
            ////var mockedConfigService = Substitute.For<IConfigCreatorService>();
            ////services.AddSingleton<>

            //IServiceProvider serviceProvider = services.BuildServiceProvider();

            //_services = serviceProvider;

        }

        public IServiceProvider getServices()
        {
            return this._services;
        }

        public void MockSharedContent()
        {
               
        }



    }
}