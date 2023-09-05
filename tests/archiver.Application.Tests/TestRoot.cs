namespace archiver.Application.Tests
{
    using archiver;
    using archiver.Application.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NSubstitute;

    [TestClass]
    public static class TestRoot
    {

        internal static IConfigCreatorService _configCreatorService;
        internal static IConsoleService _consoleService;
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
                .RegisterMockReplacement(out _configCreatorService, false)
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
                throw new Exception();
            }

            services.AddTransient(provider => mockInstance);

            return services;
        }
            

    }
}