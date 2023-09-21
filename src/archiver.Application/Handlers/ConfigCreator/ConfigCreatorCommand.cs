namespace archiver.Application.Handlers.ConfigCreator
{
    using archiver.Core;
    using archiver.Core.Enum;
    using archiver.Core.Interfaces;
    using MediatR;

    public class ConfigCreatorCommand : IRequest<ConfigCreatorResponse> { }

    public class ConfigCreatorHandler : IRequestHandler<ConfigCreatorCommand, ConfigCreatorResponse>
    {
        private readonly IConfigService _configCreatorService;


        public ConfigCreatorHandler(IConfigService configCreatorService)
        {
            _configCreatorService = configCreatorService;
        }

        public virtual async Task<ConfigCreatorResponse> Handle(ConfigCreatorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // if file path exists then return false

                return this.CheckConfigExists()
                    ? new ConfigCreatorResponse() { ConfigCreated = ConfigCreated.False }
                    : WriteNewConfigFile();

            }
            catch (Exception e)
            {
                throw new ProgramException() { ErrorCode = ErrorCodes.CONFIG_CREATION_ERROR, ErrorMessage = e.Message };
            }

        }

        public bool CheckConfigExists()
        {
            return _configCreatorService.CheckConfigExists();
        }

        public ConfigCreatorResponse WriteNewConfigFile()
        {
            try
            {
                _configCreatorService.WriteNewConfigFile();

                return new ConfigCreatorResponse() { ConfigCreated = ConfigCreated.True };

            }
            catch (Exception)
            {
                throw new ProgramException() { ErrorCode = ErrorCodes.CONFIG_CREATION_ERROR };
            }

        }
    }
}
