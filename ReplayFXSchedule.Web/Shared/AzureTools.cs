using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;

namespace ReplayFXSchedule.Web.Shared
{
    public class AzureTools
    {
        public string GetFileName(HttpPostedFileBase upload)
        {
            int indexExt = 0;
            string ext;
            string imagename = null;
            if (upload != null)
            {
                indexExt = upload.FileName.IndexOf(".");
                ext = upload.FileName.Substring(indexExt);
                imagename = Guid.NewGuid() + ext;
                uploadtoAzure(imagename, upload);
            }
            return imagename;
        }

        public string GetFileName(System.IO.Stream upload)
        {
            string ext;
            string imagename = null;
            if (upload != null)
            {
                ext = ".jpg";
                imagename = Guid.NewGuid() + ext;
                uploadtoAzure(imagename, upload);
            }
            return imagename;
        }

        public void uploadtoAzure(string filename, System.IO.Stream upload)
        {
               CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                              CloudConfigurationManager.GetSetting("StorageConnectionString"));
            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["AzureFolder"]);
            // Retrieve reference to a blob named "someimage.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{filename}");
            // Create or overwrite the "someimage.jpg" blob with contents from an upload stream.
            blockBlob.UploadFromStream(upload);
        }

        public void uploadtoAzure(string filename, HttpPostedFileBase upload)
        {
            uploadtoAzure(filename, upload.InputStream);
        }
        public void deletefromAzure(string filename)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["AzureFolder"]);
            // Retrieve reference to a blob named "someimage.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{filename}");
            // If it already exists, delete it.
            if (blockBlob.Exists())
            {
                blockBlob.Delete();
            }
        }
    }
}