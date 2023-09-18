namespace archiver.Tests
{
    using archiver;
    using archiver.Application.Interfaces;
    using archiver.Infrastructure.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;

    [TestClass]
    public static class TestRoot
    {

        internal static IConfigService _configService;
        internal static IConsoleService _consoleService;
        internal static IIOService _ioService;
        internal static IServiceScopeFactory _scopeFactory;
        internal static IMediator _mediator;
        internal static IArchiver _archiver;

        [AssemblyInitialize]
        public static void Initialize(TestContext testContext)
        {
            var config = new ConfigurationBuilder()
                .Build();

            IServiceCollection services = new ServiceCollection();

            var startup = new Startup(config);
            startup.ConfigureServices(services);

            services
                .RegisterMockReplacement(out _configService, true)
                .RegisterMockReplacement(out _ioService, true)
                .RegisterMockReplacement(out _consoleService, true)
                .RegisterMockReplacement(out _mediator, true)
                //.RegisterMockReplacement(out _archiver, true)
                ;


            _scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
            _archiver = services.BuildServiceProvider().GetRequiredService<IArchiver>();

        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            return await _mediator.Send(request);
        }


        public static IServiceCollection RegisterMockReplacement<TMock>( 
            this IServiceCollection services, 
            out TMock mockInstance,
            bool throwIfExistingDependencyIsMissing)
            where TMock : class
        {
            mockInstance = Substitute.For<TMock>();
            return services.RegisterMockReplacement(mockInstance, throwIfExistingDependencyIsMissing);
        } 

        public static IServiceCollection RegisterMockReplacement<TMock>(
            this IServiceCollection services,
            TMock mockInstance,
            bool throwIfExistingDependencyIsMissing)
            where TMock : class
        {
            var descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(TMock));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            } else if (throwIfExistingDependencyIsMissing)
            {
                throw new DI_Exception();
            }

            services.AddTransient(provider => mockInstance);

            return services;
        }
            

    }

    public class DI_Exception : Exception
    {
        public string Message { get; } = "A service needs registering";
    }
}