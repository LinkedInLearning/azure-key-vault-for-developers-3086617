using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePortal.FileService.Model
{
    public class DownloadFileInput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public FileExternalSourceInput ExternalSrouce { get; set; }
    }
   
}
