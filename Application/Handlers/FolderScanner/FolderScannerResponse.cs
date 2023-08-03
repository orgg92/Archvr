namespace Application.Handlers.FolderScanner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FolderScannerResponse
    {
        public IEnumerable<string> FileList { get; set; }
    }
}
