﻿namespace archiver.Application.Handlers
{
    using archiver.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BaseResponse
    {
        public ProgramException HandlerException { get; set; }
    }
}