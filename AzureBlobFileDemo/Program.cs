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
               
                string containerName = "mydemo1";
                string blobStorageConnectionString = "";
                string blobDirectory = "Test";
                string blobFilePath = "DefaultDemo/" + fileName;



               // Console.WriteLine($"{new UploadFileUsingMicrosoftAzureStorage().UploadFile(blobStorageConnectionString, containerName,blobDirectory, blobFilePath,filePath)}");

                Console.WriteLine($"{new UploadFileUsingAzureStorageBlob().UploadFile(blobStorageConnectionString, containerName, blobDirectory, blobFilePath, filePath)}");

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
