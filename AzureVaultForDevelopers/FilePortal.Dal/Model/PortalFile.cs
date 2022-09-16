using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FilePortal.Dal.Model
{
    public partial class PortalFile
    {
        public PortalFile()
        {
            
        }

        public Guid Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public string FileName { get; set; }
        public string? Description { get; set; }
        public string UserId { get; set; } = null!;
        public Guid? ExternalSourceId { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual ExternalFileSource ExternalFileSource { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
