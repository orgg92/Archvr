namespace archiver.Tests
{
    using archiver;
    using archiver.Application.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;

    [TestClass]
    public static class TestRoot
    {

        internal static IConfigService _configCreatorService;
        internal static IConsoleService _consoleService;
        internal static IIOService _ioService;
        internal static IServiceScopeFactory _scopeFactory;

        [AssemblyInitialize]
        public static void Initialize(TestContext testContext)
        {
            var config = new ConfigurationBuilder()
                .Build();

            IServiceCollection services = new ServiceCollection();

            var startup = new Startup(config);
            startup.ConfigureServices(services);

            services
                .RegisterMockReplacement(out _configCreatorService, true)
                .RegisterMockReplacement(out _ioService, true)
                .RegisterMockReplacement(out _consoleService, false)
                ;

            _scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();

        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            return await mediator.Send(request);
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