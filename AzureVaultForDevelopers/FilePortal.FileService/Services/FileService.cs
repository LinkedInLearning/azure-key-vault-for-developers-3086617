using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using FilePortal.Dal;
using FilePortal.Dal.Model;
using FilePortal.FileService.Config;
using FilePortal.FileService.Model;
using FilePortal.FileService.Model.Files;
using FilePortal.SecureVault;
using Microsoft.EntityFrameworkCore;

namespace FilePortal.FileService.Services
{
    public interface IFileService
    {
        Task<string> CreateDownloadLink(Guid fileId, string userId);
        Task UploadFile(Stream file, string userId, string displayName, Guid? customSourceId);
        Task<IEnumerable<FileViewModel>> GetFiles(string userId);
        Task<IEnumerable<CustomSourceViewModel>> GetCustomSources(string userId);
        Task CreateCustomSource(NewCustomSource data, string userId);
    }

    public class FileService : IFileService
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly ApplicationDbContext _db;
        private readonly IVaultService _vault;
        public FileService(ApplicationDbContext db, StorageConfig storageConfig, IVaultService vault )
        {
            _connectionString = storageConfig.ConnectionString;
            _containerName = storageConfig.ContainerName;
            _db = db;
            _vault = vault;
        }


        public async Task<IEnumerable<FileViewModel>> GetFiles(string userId)
        {
            var files = await _db.PortalFiles.Where(z => z.UserId == userId).Select(file => new FileViewModel
            {
                Id = file.Id,
                IsStoredExternaly = file.ExternalSourceId != null,
                ExternalSourceName=file.ExternalFileSource.Name,
                Name = file.DisplayName,
                CreatedOn = file.CreatedOn
            }).ToListAsync();
            return files;
        }
        public async Task UploadFile(Stream file, string userId, string displayName, Guid? customSourceId)
        {
            var externalSource = customSourceId.HasValue?_db.ExternalFileSource.Find(customSourceId):null;
            var container = await GetBlobContainer(externalSource);
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(displayName);//create unique file name
            BlobClient blobClient = container.GetBlobClient(fileName);

            file.Position = 0;
            // Upload data from the local file
            await blobClient.UploadAsync(file, true);

            var portalFile = new PortalFile
            {
                Id = Guid.NewGuid(),
                ExternalSourceId = customSourceId,
                DisplayName = displayName,
                FileName = fileName,
                CreatedOn = DateTime.UtcNow,
                UserId = userId,

            };
            _db.PortalFiles.Add(portalFile);
            _db.SaveChanges();
        }

        public async Task<string> CreateDownloadLink(Guid fileId, string userId)
        {
            var file = _db.PortalFiles.Include(f => f.ExternalFileSource).SingleOrDefault(f => f.Id == fileId && f.UserId == userId);
            if (file == null) return String.Empty;


            var container = await GetBlobContainer(file.ExternalFileSource);
            BlobClient blobClient = container.GetBlobClient(file.FileName);
            var sasBuilder = new BlobSasBuilder(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddMinutes(10))
            {
                ContentDisposition = "attachment; filename=" + file.DisplayName
            };
            var url = blobClient.GenerateSasUri(sasBuilder);
            return url.AbsoluteUri;
        }

        public async Task<IEnumerable<CustomSourceViewModel>> GetCustomSources(string userId)
        {
            var files = await _db.ExternalFileSource.Where(z => z.UserId == userId).Select(source => new CustomSourceViewModel
            {
                Id = source.Id,
                Name= source.Name,
                ContainerName = source.ContainerName,
                Description = source.Description
            }).ToListAsync();
            return files;
        }
        public async Task CreateCustomSource (NewCustomSource data, string userId)
        {
           var newSource= _db.ExternalFileSource.Add(new ExternalFileSource
            {
                Id = Guid.NewGuid(),
                ContainerName = data.ContainerName,
                Description = data.Description,
                Name = data.Name,
                UserId = userId,
                ConnectionStringKey = "ClientSecret-Storage-" + Guid.NewGuid()
            }).Entity;

            await _vault.CreateSecret(newSource.ConnectionStringKey, data.StorageConnectionString);

            _db.SaveChanges();

           

        }

        #region private

        private async Task<BlobContainerClient> GetBlobContainer(ExternalFileSource externalSource = null)
        {
            if (externalSource == null)
            {
                // Create a BlobServiceClient object which will be used to create a container client
                BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
                // Create the container and return a container client object
                var blobContainer = blobServiceClient.GetBlobContainerClient(_containerName);
                //create container if it doesn't exist
                await blobContainer.CreateIfNotExistsAsync();
                return blobContainer;
            }
            else
            {
               
                var connectionString = await _vault.GetSecret(externalSource.ConnectionStringKey);
                // Create a BlobServiceClient object which will be used to create a container client
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                // Create the container and return a container client object
                var blobContainer = blobServiceClient.GetBlobContainerClient(externalSource.ContainerName);
                //create container if it doesn't exist
                await blobContainer.CreateIfNotExistsAsync();
                return blobContainer;
            }
        }
        #endregion
    }
}
