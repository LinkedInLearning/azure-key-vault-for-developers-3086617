using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FilePortal.Dal.Model
{
    public partial class ExternalFileSource
    {
        public ExternalFileSource()
        {
            PortalFiles = new HashSet<PortalFile>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string ConnectionStringKey { get; set; }
        public string ContainerName { get; set; }
        public string UserId { get; set; } = null!;


        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<PortalFile> PortalFiles { get; set; }

    }
}
