using System.ComponentModel.DataAnnotations;

namespace FileStorageMicroService.Models
{
	public class UploadRequestModel
	{
		[Required]
		public IFormFile? File { get; set; }
        [Required]
        public string? Buckcet { get; set; }
        [Required]
        public string? Key{ get; set; }
        [Required]
        public string? ContentType { get; set; }
	}
}

