using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlobFileDemo
{
    public class UploadFileUsingAzureStorageBlob
    {
        public string UploadFile(string blobStorageConnectionString, string containerName, string blobDirectory, string blobFilePath, string filePath)
        {
            try
            {
                var blobServiceClient = new BlobServiceClient(blobStorageConnectionString);
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
                blobContainerClient.CreateIfNotExists(PublicAccessType.None);
                var blobClient = blobContainerClient.GetBlobClient($"{blobDirectory}/{blobFilePath}");
                blobClient.Upload(filePath);
                return SasTokenProvider.GetSasToken(blobStorageConnectionString, containerName, $"{blobDirectory}/{blobFilePath}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
