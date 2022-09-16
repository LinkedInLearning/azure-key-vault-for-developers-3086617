using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage;
using FilePortal.SecureVault.Config;
using Microsoft.Azure.KeyVault;
using System;
using System.Net;
using System.Text;
using static Microsoft.Azure.KeyVault.WebKey.JsonWebKeyVerifier;

namespace FilePortal.SecureVault
{
    public interface IVaultService
    {
        Task<string> CreateSecret(string secretName, string secretValue);
        Task DeleteSecret(string secretName);
        Task<string> GetSecret(string secretName);
        Task<string> Decrypt(string cipherText);
        Task<string> Encrypt(string value);
        ClientSideEncryptionOptions GetClientSideEncryptionOptions();
    }

    public class VaultService : IVaultService
    {
        public readonly string _keyName = "EncryptionDemo";
        private readonly string _vaultUrl;
        private readonly SecretClient _secretClient;
        private readonly KeyClient _keyClient;

        public VaultService(VaultConfiguration config)
        {

            _vaultUrl = config.Endpoint;
            SecretClientOptions secretOptions = new SecretClientOptions()
            {
                Retry =
                       {
                           Delay= TimeSpan.FromSeconds(2),
                           MaxDelay = TimeSpan.FromSeconds(16),
                           MaxRetries = 5,
                           Mode = RetryMode.Exponential
                }
              };
           // https://github.com/azure/azure-sdk-for-net/tree/main/sdk/keyvault/samples/keyvaultproxy/src
            secretOptions.AddPolicy(new KeyVaultProxy(TimeSpan.FromSeconds(30)), HttpPipelinePosition.PerCall);

            _secretClient = new SecretClient(new Uri(_vaultUrl), GetCredentials(), secretOptions);
            KeyClientOptions keyOptions = new KeyClientOptions
            {
                Retry =
                {
                          Delay= TimeSpan.FromSeconds(2),
                           MaxDelay = TimeSpan.FromSeconds(16),
                           MaxRetries = 5,
                           Mode = RetryMode.Exponential
                }
            };
            keyOptions.AddPolicy(new KeyVaultProxy(TimeSpan.FromSeconds(30)), HttpPipelinePosition.PerCall);
            _keyClient = new KeyClient(new Uri(_vaultUrl), GetCredentials(), keyOptions);

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

            var result = await _secretClient.SetSecretAsync(secretName, secretValue);
            return result.Value.Value;
        }
        public async Task DeleteSecret(string secretName)
        {
            if (string.IsNullOrEmpty(secretName)) return;
            await _secretClient.StartDeleteSecretAsync(secretName);
        }
        public async Task<string> Encrypt(string value)
        {
            var byteData = Encoding.Unicode.GetBytes(value);
            var encrypted = await _keyClient.GetCryptographyClient(_keyName).EncryptAsync(EncryptionAlgorithm.RsaOaep, byteData);
            var encodedText = Convert.ToBase64String(encrypted.Ciphertext);
            return encodedText;
        }
        public async Task<string> Decrypt(string cipherText)
        {
            var byteData = Convert.FromBase64String(cipherText);
            var decrypted = await _keyClient.GetCryptographyClient(_keyName).DecryptAsync(EncryptionAlgorithm.RsaOaep, byteData);
            return Encoding.Unicode.GetString(decrypted.Plaintext);
        }
        public ClientSideEncryptionOptions GetClientSideEncryptionOptions()
        {
            var key = _keyClient.GetKey(_keyName);

            var keyResolver = new KeyResolver(GetCredentials());

            var cryptoClient = new CryptographyClient(key.Value.Id, GetCredentials());

            var encryptionOptions = new ClientSideEncryptionOptions(ClientSideEncryptionVersion.V2_0)
            {
                KeyEncryptionKey = cryptoClient,
                KeyResolver = keyResolver,
                KeyWrapAlgorithm = "RSA-OAEP"
            };
            return encryptionOptions;


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