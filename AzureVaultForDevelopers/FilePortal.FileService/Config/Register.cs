using FilePortal.FileService.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePortal.FileService.Config
{
   
    public static class Register
    {
        public static void RegisterFileServices(this IServiceCollection services, StorageConfig storageConfig)
        {
            services.AddSingleton<StorageConfig>(storageConfig);
            services.AddTransient<IFileService, FilePortal.FileService.Services.FileService>();
        }
    }
}
