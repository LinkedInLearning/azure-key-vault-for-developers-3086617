using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePortal.Dal.Model
{
    
    public partial class ApplicationUser : IdentityUser
    {
         public ApplicationUser()
        {
            ExternalFileSources = new HashSet<ExternalFileSource>();
            PortalFiles = new HashSet<PortalFile>();
        }
        public virtual ICollection<ExternalFileSource> ExternalFileSources { get; set; }
        public virtual ICollection<PortalFile> PortalFiles { get; set; }

    }
}
