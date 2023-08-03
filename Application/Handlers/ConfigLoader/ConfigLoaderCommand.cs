namespace Application.Handlers.ConfigLoader
{
    using MediatR;
    using System.ComponentModel;

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
            if (CheckConfigExists() && CheckConfigHasBeenTouched())
            {
                LoadConfig();

                return new ConfigLoaderResponse() { ConfigLoaded = true };
            } else
            {
                return new ConfigLoaderResponse() { ConfigLoaded = false };
            }


        }

        //check file has been modified since it's creation, otherwise the file will only contain the default config template so no point loading
        public bool CheckConfigHasBeenTouched()
        {
            return File.GetCreationTimeUtc(SharedContent.ConfigFullPath) != File.GetLastWriteTimeUtc(SharedContent.ConfigFullPath);
        }

        public bool CheckConfigExists()
        {
            return Directory.Exists(SharedContent.ConfigDirPath) && Directory.Exists(SharedContent.LogPath);
        }

        public void LoadConfig()
        {
            int i = 0;
            foreach (string line in System.IO.File.ReadLines($"{SharedContent.ConfigFullPath}"))
            {
                SharedContent.configValues[i] = line.Split("=")[1].Replace("'", "");
                i++;
            }

            SharedContent.TargetDrive = SharedContent.configValues[0];
            SharedContent.DestinationDrive = SharedContent.configValues[1];
            SharedContent.RetryCount = SharedContent.configValues[2];
            SharedContent.LogProgressToConsole = Boolean.Parse(SharedContent.configValues[3]);
            SharedContent.OutputLocation = SharedContent.configValues[4];
            SharedContent.DirListFileLocation = SharedContent.configValues[5];
            SharedContent.ConsoleHeight = SharedContent.configValues[6];
            SharedContent.ConsoleWidth = SharedContent.configValues[7];

            SetConsoleSize();

            SharedContent.ResponsiveSpacer = new String('*', int.Parse(SharedContent.ConsoleWidth));
        }

        public void SetConsoleSize()
        {
            var consoleHeight = int.TryParse(SharedContent.ConsoleHeight, out var h) ? h : 25;
            var consoleWidth = int.TryParse(SharedContent.ConsoleWidth, out var w) ? w : 100;
            SharedContent.ConsoleHeight = consoleHeight.ToString();
            SharedContent.ConsoleWidth = consoleWidth.ToString();

            Console.SetWindowSize(consoleWidth, consoleHeight);
        }
    }
}
