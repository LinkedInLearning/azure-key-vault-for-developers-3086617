using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Security.KeyVault.Secrets;
using FilePortal.SecureVault.Config;
using Microsoft.Azure.KeyVault;
using System;
using System.Text;

namespace FilePortal.SecureVault
{
    public interface IVaultService
    {
        Task<string> CreateSecret(string secretName, string secretValue);
        Task DeleteSecret(string secretName);
        Task<string> GetSecret(string secretName);
        Task<string> Decrypt(string cipherText);
        Task<string> Encrypt(string value);
    }

    public class VaultService : IVaultService
    {
       
        private readonly string _vaultUrl;
        private readonly SecretClient _secretClient;
        private readonly KeyClient _keyClient;
        private readonly string _keyName = "EncryptionDemo";

        public VaultService(VaultConfiguration config)
        {
          
            _vaultUrl = config.Endpoint;
            _secretClient = new SecretClient(new Uri(_vaultUrl), GetCredentials());
            _keyClient = new KeyClient(new Uri(_vaultUrl), GetCredentials());

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
        public async Task<string> Encrypt(string value)
        {
            var byteData = Encoding.Unicode.GetBytes(value);
            var encrypted = await _keyClient.GetCryptographyClient(_keyName).EncryptAsync(EncryptionAlgorithm.RsaOaep, byteData);
            var encodedText = Convert.ToBase64String(encrypted.Ciphertext);
            return encodedText;
        }
        public async Task<string> Decrypt( string cipherText)
        {
            var byteData = Convert.FromBase64String(cipherText);
            var decrypted = await _keyClient.GetCryptographyClient(_keyName).DecryptAsync(EncryptionAlgorithm.RsaOaep, byteData);
            return Encoding.Unicode.GetString(decrypted.Plaintext);
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