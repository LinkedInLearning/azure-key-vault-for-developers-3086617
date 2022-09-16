using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePortal.FileService.Model.Files
{
    public class FileViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsStoredExternaly { get; set; }
        public string ExternalSourceName { get; internal set; }
    }
}
