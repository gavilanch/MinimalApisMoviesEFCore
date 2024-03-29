
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MinimalAPIsMovies.Services
{
    public class AzureFileStorage(IConfiguration configuration) : IFileStorage
    {
        private string connectionString = configuration.GetConnectionString("AzureStorage")!;

        public async Task Delete(string? route, string container)
        {
            if (string.IsNullOrEmpty(route))
            {
                return;
            }

            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();
            var fileName = Path.GetFileName(route);
            var blob = client.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> Store(string container, IFormFile file)
        {
            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(PublicAccessType.Blob);

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var blob = client.GetBlobClient(fileName);
            BlobHttpHeaders blobHttpHeaders = new();
            blobHttpHeaders.ContentType = file.ContentType;
            await blob.UploadAsync(file.OpenReadStream(), blobHttpHeaders);
            return blob.Uri.ToString();
        }
    }
}
