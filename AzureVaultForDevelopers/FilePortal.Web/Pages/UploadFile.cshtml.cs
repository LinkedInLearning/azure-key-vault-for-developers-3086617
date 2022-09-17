using FilePortal.FileService.Model.Files;
using FilePortal.FileService.Services;
using FilePortal.Web.Pages.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Security.Principal;
namespace FilePortal.Web.Pages
{
    [Authorize]
    public class UploadFileModel : BasePageModel
    {
       
        private readonly ILogger<DownloadFileModel> _logger;
        private readonly IFileService _fileService;

        public UploadFileModel(ILogger<DownloadFileModel> logger,  IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;
        }
        public IEnumerable<CustomSourceViewModel> Sources { get; set; }
        public async Task OnGetAsync()
        {

            this.Sources = await _fileService.GetCustomSources(CurrentUserId);
        }

        [BindProperty]
        public IFormFile Upload { get; set; }
        [BindProperty]
        public Guid? CustomSourceId { get; set; }

       
        public async Task<IActionResult> OnPostAsync()
        {
            using (var memoryStream = new MemoryStream())
            {
                await Upload.CopyToAsync(memoryStream);
                await _fileService.UploadFile(memoryStream, CurrentUserId, Upload.FileName,CustomSourceId);
            }
            return RedirectToPage(pageName: "Index");

        }




    }
}