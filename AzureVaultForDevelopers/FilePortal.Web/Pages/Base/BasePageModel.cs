using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace FilePortal.Web.Pages.Base
{
    public class BasePageModel:PageModel
    {
        public string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
        
    }
}
