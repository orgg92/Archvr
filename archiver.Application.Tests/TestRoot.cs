namespace archiver.Application.Tests
{
    using archiver;
    using archiver.Application.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;

    [TestClass]
    public static class TestRoot
    {

        internal static Mock<IConfigCreatorService> _configCreatorService;
        internal static Mock<IConsoleService> _consoleService;
        internal static  IServiceScopeFactory _scopeFactory;

        private static MockRepository _mockRepository;
        // private static SubstituteFactory _substituteFactory; -- Shift mocking from Moq to NSubstitute

        [AssemblyInitialize]
        public static void Initialize(TestContext testContext)
        {
            var config = new ConfigurationBuilder()
                .Build();

            IServiceCollection services = new ServiceCollection();

            _mockRepository = new MockRepository(MockBehavior.Loose);

            var startup = new Startup(config);
            startup.ConfigureServices(services);

            services
                .RegisterMockReplacement(out _configCreatorService, false)
                .RegisterMockReplacement(out _consoleService, false)
                ;

            _scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();

        }

        public static Task Reset()
        {
            _configCreatorService.Reset();

            return Task.FromResult(true);
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            return await mediator.Send(request);
        }


        public static IServiceCollection RegisterMockReplacement<TMock>( 
            this IServiceCollection services, 
            out Mock<TMock> mockInstance,
            bool throwIfExistingDependencyIsMissing)
            where TMock : class
        {
            mockInstance = _mockRepository.Create<TMock>();
            return services.RegisterMockReplacement(mockInstance, throwIfExistingDependencyIsMissing);
        } 

        public static IServiceCollection RegisterMockReplacement<TMock>( this IServiceCollection services,
            bool throwIfExistingDependencyIsMissing)
            where TMock : class
        {
            var mock = _mockRepository.Create<TMock>();

            services.RegisterMockReplacement(mock, throwIfExistingDependencyIsMissing);

            return services;
        }

        public static IServiceCollection RegisterMockReplacement<TMock>(
            this IServiceCollection services,
            IMock<TMock> mockInstance,
            bool throwIfExistingDependencyIsMissing)
            where TMock : class
        {
            var descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(TMock));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            } else if (throwIfExistingDependencyIsMissing)
            {
                throw new Exception();
            }

            services.AddTransient(provider => mockInstance.Object);

            return services;
        }
            

    }
}