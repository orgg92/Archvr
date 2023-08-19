namespace archiver.Application.Tests
{
    using global::Application.Interfaces;
    using global::Application.Services;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NSubstitute;
    using Ryker;

    [TestClass]
    public class TestRoot
    {
        private IConsoleService _consoleService;
        private ILoggerService _loggerService;

        public Startup startup;

        [TestInitialize]
        public void Start()
        {
            var config = new ConfigurationBuilder()
                            .Build();

            IServiceCollection services = new ServiceCollection();

            startup = new Startup(config);
            startup.ConfigureServices(services);

            services.AddTransient<IConsoleService, ConsoleService>();

            this._consoleService = Substitute.For<IConsoleService>();
            
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var consoleDescription = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IConsoleService));
            services.Remove(consoleDescription);

            var mockConsoleService = Substitute.For<IConsoleService>();



            //services.AddTransient<mockConsoleService>();


        }



    }
}