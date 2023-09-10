namespace archiver.Application.Handlers.ConfigLoader
{
    using archiver.Application.Interfaces;
    using archiver.Core;
    using MediatR;

    public class ConfigLoaderCommand : IRequest<ConfigLoaderResponse> { }

    public class ConfigLoaderHandler : IRequestHandler<ConfigLoaderCommand, ConfigLoaderResponse>
    {

        private readonly IConfigService _configService;

        public ConfigLoaderHandler(IConfigService configService)
        {
            _configService = configService;
        }

        public async Task<ConfigLoaderResponse> Handle(ConfigLoaderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (_configService.CheckConfigExists() && _configService.CheckConfigHasBeenTouched())
                {
                    _configService.LoadConfig();

                    SetConsoleSize();

                    return new ConfigLoaderResponse() { ConfigLoaded = true };
                }
                else
                {
                    return new ConfigLoaderResponse() { ConfigLoaded = false };
                }
            } catch (Exception e)
            {
                throw new ProgramException() { ErrorCode = ErrorCodes.CONFIG_LOAD_ERROR, ErrorMessage = e.Message };
            }
        }

        public void SetConsoleSize()
        {
            if (System.Environment.OSVersion.Platform.ToString().ToLower().Contains("win"))
            {
                Console.SetWindowSize(ProgramConfig.ConsoleWidth, ProgramConfig.ConsoleHeight);
            } else
            {
                ProgramConfig.ConsoleHeight = Console.WindowHeight;
                ProgramConfig.ConsoleWidth = Console.WindowWidth;
            }
        }
    }
}
