namespace archiver.Application.Handlers.ConfigCreator
{
    public class ConfigCreatorResponse : BaseResponse
    {
        public ConfigCreated ConfigCreated { get; set; }
    }

    public enum ConfigCreated
    {
        True,
        False,
        Error

    }
}
