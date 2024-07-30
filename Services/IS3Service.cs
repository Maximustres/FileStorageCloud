using System;
using FileStorageMicroService.Models;

namespace FileStorageMicroService.Services
{
	public interface IS3Service
	{
        Task UploadFileAsync(UploadRequestModel uploadRequest);
        Task<Stream> DownloadFileAsync(string bucketName, string key);
    }
}

