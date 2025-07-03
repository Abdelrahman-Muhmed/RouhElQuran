using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.HelperModel.FileModel
{
	public class FileUpload
	{
        [Required]
        [Display(Name="File")]
        public List<IFormFile>? FormFile { get; set; }
		public string? Note { get; set; }

		public string? UntrustedName { get; set; }
		public byte[]? Content { get; set; }

		public long Size { get; set; }

		public DateTime UploadDT { get; set; }

		public int? AppUserId { get; set; }


		public int? CourseId { get; set; }
	}
}
