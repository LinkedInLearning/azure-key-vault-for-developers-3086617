using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePortal.FileService.Model
{
    public class FileExternalSourceInput
    {
        public string ConnectionStringKey { get; set; }
        public string ContainerName { get; set; }
        public Guid Id { get; internal set; }
    }
}
