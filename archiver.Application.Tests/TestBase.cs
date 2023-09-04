namespace archiver.Application.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static global::archiver.Application.Tests.TestRoot;

    public abstract class TestBase
    {
        [TestInitialize]
        public virtual async Task Initialize()
        {
            await Reset();
        }
    }
}
