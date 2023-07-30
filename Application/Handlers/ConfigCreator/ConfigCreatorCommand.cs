namespace Application.Handlers.ConfigCreator
{
    using MediatR;

    public class ConfigCreatorCommand : IRequest<ConfigCreatorResponse>
    {

    }

    public class ConfigCreatorHandler : IRequestHandler<ConfigCreatorCommand, ConfigCreatorResponse>
    {
        public ConfigCreatorHandler()
        {

        }

        public async Task<ConfigCreatorResponse> Handle(ConfigCreatorCommand request, CancellationToken cancellationToken)
        {
            return new ConfigCreatorResponse();
        }
    }
}
