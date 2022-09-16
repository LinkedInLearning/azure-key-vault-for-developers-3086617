using FilePortal.FileService.Model.Files;
using FilePortal.FileService.Services;
using FilePortal.SecureVault;
using FilePortal.Web.Pages.Base;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace FilePortal.Web.Pages;

[Authorize]
public class IndexModel : BasePageModel
{

    private readonly ILogger<IndexModel> _logger;
    private readonly IFileService _fileService;

    public IndexModel(ILogger<IndexModel> logger, IFileService fileService)
    {
        _logger = logger;
        _fileService = fileService;
    }


    public IEnumerable<FileViewModel> Files;
    public async Task OnGetAsync()
    {

        this.Files = await _fileService.GetFiles(CurrentUserId);
    }

}
