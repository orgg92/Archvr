namespace Application.Handlers.ConfigLoader
{
    using MediatR;

    public class ConfigLoaderCommand : IRequest<ConfigLoaderResponse>
    {

    }

    public class ConfigLoaderHandler : IRequestHandler<ConfigLoaderCommand, ConfigLoaderResponse>
    {
        public ConfigLoaderHandler()
        {

        }

        public async Task<ConfigLoaderResponse> Handle(ConfigLoaderCommand request, CancellationToken cancellationToken)
        {
            return new ConfigLoaderResponse();
        }
    }
}
