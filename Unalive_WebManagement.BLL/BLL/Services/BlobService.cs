using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Unalive_WebManagement.BLL.Interfaces;

namespace Unalive_WebManagement.BLL.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"];
            var containerName = configuration["AzureStorage:ContainerName"];

            if (string.IsNullOrEmpty(connectionString) || connectionString == "hehe")
            {
                throw new ArgumentException("Azure Storage ConnectionString is missing or invalid in appsettings.json. It should look like 'DefaultEndpointsProtocol=https;AccountName=...;'");
            }

            _containerClient = new BlobContainerClient(connectionString, containerName);
        }

        public async Task<string> UploadImageAsync(IFormFile file, string fileName)
        {
            // Ensure container exists before uploading
            await _containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = _containerClient.GetBlobClient(fileName);
            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, new BlobHttpHeaders
            {
                ContentType = file.ContentType
            });
            return blobClient.Uri.ToString();
        }

        public async Task DeleteImageAsync(string fileName)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}