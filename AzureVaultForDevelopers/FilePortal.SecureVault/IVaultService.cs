using Azure.Identity;
using FilePortal.SecureVault.Config;
using System;

namespace FilePortal.SecureVault
{
    public interface IVaultService
    {
        Task<string> CreateSecret(string secretName, string secretValue);
        Task DeleteSecret(string secretName);
        Task<string> GetSecret(string secretName);

    }

    public class VaultService : IVaultService
    {
        private readonly string _vaultClientId;
        private readonly string _tenantId;
        private readonly string _vaultClientSecret;
        private readonly string _vaultUrl;


        public VaultService(VaultConfiguration config)
        {
            _tenantId = config.TenantId;
            _vaultClientId = config.ClientId;
            _vaultClientSecret = config.ClientSecret;
            _vaultUrl = config.Endpoint;
            
        }


        public async Task<string> GetSecret(string secretName)
        {
            if (string.IsNullOrEmpty(secretName)) return string.Empty;

            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                //log exceptions
                return string.Empty;
            }
        }
        public async Task<string> CreateSecret(string secretName, string secretValue)
        {
            if (string.IsNullOrEmpty(secretName) || string.IsNullOrEmpty(secretValue)) throw new InvalidOperationException("Wrong secret name or value was provided");

            throw new NotImplementedException();
        }
        public async Task DeleteSecret(string secretName)
        {
            if (string.IsNullOrEmpty(secretName)) return;
            throw new NotImplementedException();
        }

        #region private

        private ClientSecretCredential GetCredentials()
        {
            var cred = new ClientSecretCredential(_tenantId, _vaultClientId, _vaultClientSecret);
            return cred;
        }
        #endregion
    }
}