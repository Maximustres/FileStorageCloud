using System;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using FileStorageMicroService.Models;
using Microsoft.Extensions.Options;

namespace FileStorageMicroService.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly ILogger<S3Service> _logger;

        public S3Service(IAmazonS3 s3Client, ILogger<S3Service> logger)
        {
            _s3Client = s3Client;
            _logger = logger;
        }

        public async Task UploadFileAsync(UploadRequestModel uploadRequest)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_s3Client);

                using var newMemoryStream = new MemoryStream();

                uploadRequest.File.CopyTo(newMemoryStream);

                await fileTransferUtility.UploadAsync(new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = uploadRequest.Key,
                    BucketName = uploadRequest.Buckcet,
                    ContentType = uploadRequest.ContentType
                });
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError(ex, $"File error uploaded to S3 exception: {ex}");
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"File error uploaded exception: {ex}");
                throw;
            }
   
        }

        public async Task<Stream> DownloadFileAsync(string bucketName, string key)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                };

                using var response = await _s3Client.GetObjectAsync(request);
                var responseStream = response.ResponseStream;

                using var memoryStream = new MemoryStream();
                await responseStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                return memoryStream;
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while downloading the file from S3. Key: {key}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while downloading the file. Key: {key}");
                throw;
            }
        }

    }

}

