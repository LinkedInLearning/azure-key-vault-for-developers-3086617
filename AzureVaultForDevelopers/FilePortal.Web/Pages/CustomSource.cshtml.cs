using FilePortal.FileService.Model.Files;
using FilePortal.FileService.Services;
using FilePortal.Web.Pages.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Principal;
namespace FilePortal.Web.Pages
{
    [Authorize]
    public class CustomSourceModel : BasePageModel
    {
       
        private readonly IFileService _fileService;

        public CustomSourceModel(  IFileService fileService)
        {
            _fileService = fileService;
        }

        [BindProperty]
        public NewCustomSource NewCustomSource { get; set; }

        public IEnumerable<CustomSourceViewModel> Sources { get; set; }
        public async Task OnGetAsync()
        {

            this.Sources = await _fileService.GetCustomSources(CurrentUserId);
        }
        public async Task OnPostAsync()
        {
            await _fileService.CreateCustomSource(NewCustomSource, CurrentUserId);
            this.Sources = await _fileService.GetCustomSources(CurrentUserId);

        }




    }
   
   
}