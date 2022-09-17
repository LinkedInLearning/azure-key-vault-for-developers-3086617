using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePortal.SecureVault.Config
{
    public static class Register
    {
        public static void RegisterVaultServices(this IServiceCollection services, VaultConfiguration vaultConfig)
        {
            services.AddSingleton<VaultConfiguration>(vaultConfig);
            services.AddSingleton<IVaultService, VaultService>();

        }
    }
}
