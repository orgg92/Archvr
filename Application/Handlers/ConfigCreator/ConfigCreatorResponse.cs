namespace archiver.Application.Handlers.ConfigCreator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
