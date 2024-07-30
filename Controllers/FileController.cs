using FileStorageMicroService.Models;
using FileStorageMicroService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileStorageMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IS3Service _s3Service;
        private readonly ILogger<FileController> _logger;

        public FileController(
            IS3Service s3Service,
            ILogger<FileController> logger)
		{
            _s3Service = s3Service;
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(UploadRequestModel uploadRequest)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                if (uploadRequest.File == null || uploadRequest.File.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                await _s3Service.UploadFileAsync(uploadRequest);
                _logger.LogInformation("File uploaded successfully");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while uploading the file from cloudStorage. Key: {uploadRequest.Key}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Internal server error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError,
                });
            }

        }

        [HttpGet("download/{bucketName}/{key}")]
        [ResponseCache(Duration = 300)]
        public async Task<IActionResult> DownloadFile(string bucketName, string key)
        {
            try
            {
                var stream = await _s3Service.DownloadFileAsync(bucketName, key);
                if (stream == null)
                {
                    return NotFound();
                }

                return File(stream, "application/octet-stream", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while downloading the file from cloudStorage. Key: {key}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Internal server error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError,
                });
            }
        }
    }
}

