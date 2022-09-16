using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
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

        private readonly SecretClient _secretClient;

        public VaultService(VaultConfiguration config)
        {
            _tenantId = config.TenantId;
            _vaultClientId = config.ClientId;
            _vaultClientSecret = config.ClientSecret;
            _vaultUrl = config.Endpoint;
            _secretClient= new SecretClient(new Uri(_vaultUrl), GetCredentilas());
        }


        public async Task<string> GetSecret(string secretName)
        {
            if (string.IsNullOrEmpty(secretName)) return string.Empty;

            

            try
            {
                var secret = await _secretClient.GetSecretAsync(secretName);
                return secret.Value.Value;
            }
            catch (Exception ex)
            {
                // catch all possible error
                return string.Empty;
            }
        }
        public async Task<string> CreateSecret(string secretName, string secretValue)
        {
            if (string.IsNullOrEmpty(secretName) || string.IsNullOrEmpty(secretValue)) throw new InvalidOperationException("Wrong secret name or value was provided");
 
            var result = await _secretClient.SetSecretAsync(secretName, secretValue);

            return result.Value.Value;
        }
        public async Task DeleteSecret(string secretName)
        {
            if (string.IsNullOrEmpty(secretName)) return;
             await _secretClient.StartDeleteSecretAsync(secretName);
        }

        #region private

        private ClientSecretCredential GetCredentilas()
        {
            var cred = new ClientSecretCredential(_tenantId, _vaultClientId, _vaultClientSecret);
            return cred;
        }
        #endregion
    }
}