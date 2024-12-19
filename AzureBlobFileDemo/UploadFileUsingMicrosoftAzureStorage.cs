using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace AzureBlobFileDemo
{
    public  class UploadFileUsingMicrosoftAzureStorage
    {
        public string UploadFile(string blobStorageConnectionString, string containerName, string blobDirectory, string blobFilePath, string filePath)
        {
            try
            {
                CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(blobStorageConnectionString);

                var blobClient = StorageAccount.CreateCloudBlobClient();
                // Create a container for organizing blobs within the storage account.
                var container = blobClient.GetContainerReference(containerName);
                //container.CreateIfNotExists();
                var directory = container.GetDirectoryReference(blobDirectory);
                var blob = directory.GetBlockBlobReference(blobFilePath);

                //Add content type for blob upload

                //using (Stream _stream = new MemoryStream(fileBytes))
                //{

                blob.UploadFromFile(filePath);
                //}

                /* after regenerate storage key it will be work */
                return SasTokenProvider.GetSasToken(blobStorageConnectionString, containerName, $"{blobDirectory}/{blobFilePath}");

                //  this toke will not work after regenerqate storage key
                //var token = GetAccessToken(container);

                //return $"{new Uri(blob.Uri, token)}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        private static string GetAccessToken(CloudBlobContainer container)
        {
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions();

            containerPermissions.PublicAccess = BlobContainerPublicAccessType.Off;

            container.SetPermissions(containerPermissions);

            string token = container.GetSharedAccessSignature(new SharedAccessBlobPolicy
            {
                SharedAccessExpiryTime = DateTime.MaxValue,
                Permissions = SharedAccessBlobPermissions.Read,
            });

            #region Create HrOne Policy if not exists
            if (!containerPermissions.SharedAccessPolicies.ContainsKey("HrOne"))
            {
                containerPermissions.SharedAccessPolicies.Add("HrOne", new SharedAccessBlobPolicy
                {
                    SharedAccessExpiryTime = DateTime.MaxValue,
                    Permissions = SharedAccessBlobPermissions.Read
                });
                container.SetPermissions(containerPermissions);
            }
            #endregion
            return token;
        }

    }
}
