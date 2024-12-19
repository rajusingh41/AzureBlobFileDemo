using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using System.Net;

namespace AzureBlobFileDemo
{
    public static  class SasTokenProvider
    {
        public static string GetSasToken(string connectionString, string containerName, string blobName)
        {
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            // set properties on BlobSasBuilder class
            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = containerName,
                //add IPRange using a string ip address
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTime.MaxValue, // Expiration time for the SAS token
            };

            // set the required permissions on the SAS token
            sasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.Write);

            // add resource specific properties and generate the SAS
            //if (sasResourceType == SasResourceType.Container)
            //{
            //    //Create token at the container level
            //    sasBuilder.Resource = "c";
            //    sasToken = containerClient.GenerateSasUri(sasBuilder);
            //}
            //else if (sasResourceType == SasResourceType.Blob)
            //{
                //Create token at the blob level
                sasBuilder.Resource = "b";
                sasBuilder.BlobName = blobName;
               var sasToken = blobClient.GenerateSasUri(sasBuilder);
            //}
            Console.WriteLine($"Generated SAS token: {sasToken}");
            return sasToken.ToString();

        }
    }
}
