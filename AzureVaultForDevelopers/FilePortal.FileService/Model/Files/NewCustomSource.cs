using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePortal.FileService.Model.Files
{
  
    public class NewCustomSource
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public string ContainerName { get; set; }
        [Required]
        public string StorageConnectionString { get; set; }
        public string Description { get; set; }
    }
}
