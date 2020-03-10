using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AdGate.Controllers
{
    public class ProfilePictureController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private CloudBlobContainer GetContainer()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.Build();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(configuration["ConnectionStrings:BlobConnection"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("profile-pictures-container");
            container.CreateIfNotExistsAsync();
            return container;
        }

        private Image GetImage(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                using (var img = Image.FromStream(memoryStream))
                {
                    return img;
                }
            }
        }

        public async Task<string> Upload(IFormFile file, string asName)
        {
            CloudBlobContainer container = GetContainer();
            CloudBlockBlob blob = container.GetBlockBlobReference(asName);
            blob.Properties.ContentType = file.ContentType;
            using (Stream fileStream = file.OpenReadStream())
            {
                await blob.UploadFromStreamAsync(fileStream);
            }
            return blob.Uri.AbsoluteUri;
        }

        public List<string> List()
        {
            CloudBlobContainer container = GetContainer();
            List<string> blobs = new List<string>();
            BlobResultSegment resultSegment = container.ListBlobsSegmentedAsync(null).Result;
            foreach (IListBlobItem item in resultSegment.Results)
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blobBlock = (CloudBlockBlob) item;
                    blobs.Add(blobBlock.Uri.AbsoluteUri.ToString());
                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob blobPage = (CloudPageBlob) item;
                    blobs.Add(blobPage.Uri.AbsoluteUri.ToString());
                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory blobDirectory = (CloudBlobDirectory) item;
                    blobs.Add(blobDirectory.Uri.AbsoluteUri.ToString());
                }
            }
            return blobs;
        }

        public string GetProfilePicture(string uri)
        {
            CloudBlobContainer container = GetContainer();
            CloudBlockBlob blob = container.GetBlockBlobReference(uri);
            return blob.Uri.AbsoluteUri.ToString();
            //using(var memoryStream = new MemoryStream())
            //{
            //    await blob.DownloadToStreamAsync(memoryStream);
            //    return Image.FromStream(memoryStream);
            //}
        }

        public async void DeleteProfilePicture(string uri)
        {
            CloudBlobContainer container = GetContainer();
            CloudBlockBlob blob = container.GetBlockBlobReference(uri);
            await blob.DeleteAsync();
        }
    }
}