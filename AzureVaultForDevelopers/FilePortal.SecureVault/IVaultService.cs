﻿using Azure.Identity;
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
       
        private readonly string _vaultUrl;
        private readonly SecretClient _secretClient;

        public VaultService(VaultConfiguration config)
        {
          
            _vaultUrl = config.Endpoint;
            _secretClient = new SecretClient(new Uri(_vaultUrl), GetCredentials());

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
                //log exceptions
                return string.Empty;
            }
        }
        public async Task<string> CreateSecret(string secretName, string secretValue)
        {
            if (string.IsNullOrEmpty(secretName) || string.IsNullOrEmpty(secretValue)) throw new InvalidOperationException("Wrong secret name or value was provided");

            var result=await _secretClient.SetSecretAsync(secretName, secretValue);
            return result.Value.Value;
        }
        public async Task DeleteSecret(string secretName)
        {
            if (string.IsNullOrEmpty(secretName)) return;
            await _secretClient.StartDeleteSecretAsync(secretName);
        }

        #region private

        private DefaultAzureCredential GetCredentials()
        {
            var cred = new DefaultAzureCredential();
            return cred;
        }
        #endregion
    }
}