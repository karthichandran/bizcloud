using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReProServices.Infrastructure.GoogleDrive
{
    public class DriverService
    {
        private string PathToServiceAccountKeyFile = @"C:\Dev\DriveAccess\reprodrive-955e16186739.json";
         private const string filePath = @"GoogleDrive\repro-documents-2eb3e6f98a0c.json";
        //client_secret_1055780356051-s6ejr6c7reke73kp5bopqg8sofrp6a9o.apps.googleusercontent.com
      //  private const string filePath = @"GoogleDrive\biz-key.json";
        public async Task<string> AddFile(string fileName,string fileType, MemoryStream ms) {
           // PathToServiceAccountKeyFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"GoogleDrive\reprodrive-955e16186739.json");
           // PathToServiceAccountKeyFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"GoogleDrive\prestige-storage-354906-ec7dc385c854.json");
            PathToServiceAccountKeyFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), filePath);
            var credential = GoogleCredential.FromFile(PathToServiceAccountKeyFile)
                .CreateScoped(DriveService.ScopeConstants.Drive);

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {              
                Name= fileName,                
                Parents = new List<string>() { "1cDI1e-NSEbJSCEzIl2Ale0RFQ3WBMKRX" }//folder id  repro_docs
            };
            string uploadedFileId;
            // Create a new file, with metadata and stream.
            var request = service.Files.Create(fileMetadata, ms, fileType);
            request.Fields = "*";
            var results = await request.UploadAsync(CancellationToken.None);

            if (results.Status == UploadStatus.Failed)
            {
                Console.WriteLine($"Error uploading file: {results.Exception.Message}");
            }

            // the file id of the new file we created
            uploadedFileId = request.ResponseBody?.Id;

            return uploadedFileId;            
        }

        public MemoryStream GetFile(string id) {

            try
            {
                PathToServiceAccountKeyFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), filePath);

                var credential = GoogleCredential.FromFile(PathToServiceAccountKeyFile)
                    .CreateScoped(DriveService.ScopeConstants.Drive);

                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential
                });

                //var request = service.Files.Get("1n1Y80fq6jwkk6InMLUyu5D9ZLyzIPCBA");
                var request = service.Files.Get(id);
                var stream = new MemoryStream();

                // Add a handler which will be notified on progress changes.
                // It will notify on each chunk download and when the
                // download is completed or failed.
                request.MediaDownloader.ProgressChanged +=
                    progress =>
                    {
                        switch (progress.Status)
                        {
                            case DownloadStatus.Downloading:
                                {
                                    Console.WriteLine(progress.BytesDownloaded);
                                    break;
                                }
                            case DownloadStatus.Completed:
                                {
                                    Console.WriteLine("Download complete.");
                                    break;
                                }
                            case DownloadStatus.Failed:
                                {
                                    Console.WriteLine("Download failed.");
                                    break;
                                }
                        }
                    };


                request.Download(stream);

                return stream;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public bool DeleteFile(string id) {

            PathToServiceAccountKeyFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), filePath);

            var credential = GoogleCredential.FromFile(PathToServiceAccountKeyFile)
                .CreateScoped(DriveService.ScopeConstants.Drive);

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

            var request = service.Files.Delete(id);
            var staus= request.Execute();

            return true;
        }
    }
}
