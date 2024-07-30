using System;
namespace FileStorageMicroService.Models
{
	public class S3Options
	{
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string RegionEndpoint { get; set; }
    }
}

