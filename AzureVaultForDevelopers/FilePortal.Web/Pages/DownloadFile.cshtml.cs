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
    public class DownloadFileModel : BasePageModel
    {
       
        private readonly ILogger<DownloadFileModel> _logger;
        private readonly IFileService _fileService;

        public DownloadFileModel(ILogger<DownloadFileModel> logger,  IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;
        }

        public async Task<IActionResult> OnGetAsync()
        {

            var file = await _fileService.DownloadFile(this.Id, CurrentUserId);
            return File(file.FileData.ToArray(), "application/octet-stream", file.FileName);

        }

        [FromQuery(Name = "id")]
        public Guid Id { get; set; }
      


       
    }
}