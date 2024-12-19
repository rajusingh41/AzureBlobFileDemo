using Azure.Storage.Blobs;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace AzureBlobFileDemo
{
    internal class Program
    {
        static   void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            try
            {
                var filePath = "C:/Users/DELL/Downloads/sample-4.pdf";
                var fileName = $"testPdfFile{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}.pdf";
                byte[] fileBytes = File.ReadAllBytes(filePath);
                string containerName = "mydemo1";

                // string containerName = "invoices";
                // string blobStorageConnectionString = "
                string blobStorageConnectionString = "";
                CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(blobStorageConnectionString);

                var blobClient = StorageAccount.CreateCloudBlobClient();
                // Create a container for organizing blobs within the storage account.
                var container = blobClient.GetContainerReference(containerName);
                //container.CreateIfNotExists();
                var directory = container.GetDirectoryReference("Test");
                var dbFileName = "DefaultDemo/" + fileName;
                var blob = directory.GetBlockBlobReference(dbFileName);

                //Add content type for blob upload

                //using (Stream _stream = new MemoryStream(fileBytes))
                //{

                    blob.UploadFromFile(filePath);
                //}


                var sakToken = SasTokenProvider.GetSasToken(blobStorageConnectionString, containerName, "Test/"+dbFileName);


                // string token = GetAccessToken(container);
                //  Console.WriteLine($"{new Uri(blob.Uri, sakToken)}");

                Console.WriteLine(sakToken);
            }
            catch (Exception ex)
            {
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
