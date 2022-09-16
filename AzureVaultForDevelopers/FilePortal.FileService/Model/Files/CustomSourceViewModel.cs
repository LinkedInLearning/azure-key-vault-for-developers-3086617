using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePortal.FileService.Model.Files
{
    public class CustomSourceViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string ConnectionStringKey { get; set; }
        public string ContainerName { get; set; }
    }
}
