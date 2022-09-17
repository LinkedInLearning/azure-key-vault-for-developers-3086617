using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePortal.FileService.Model.Files
{
    public class DownloadFileViewModel
    {
        public MemoryStream FileData { get; set; }
        public string FileName { get; set; }
    }
}
