using Core.HelperModel;
using Core.IRepo;
using Core.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Stripe.Terminal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Core.HelperModel.FileModel;

namespace Service.Helper.FileUploadHelper
{

	public static class FileHelper
	{
		private static ModelStateDictionary modelState = new ModelStateDictionary();

		private static string _StordFilesPath = "F:\\Files";

		private static long _StreemedFileSizeLimite = 524288000;

		private static long _BufferedFileSizeLimite = 2097152;

		//for check on Chars if we are found char like "-,*,} , ..... ==> secure xss injection"
		private static readonly byte[] _allowedChars = { };

		private static readonly string[] _permittedExtensions = { ".txt", ".jpg", ".jpeg", ".png", ".mp4", ".mp3", ".pdf" };

		// Get the default form options so that we can use them to set the default 
		// limits for request body data.
		private static readonly FormOptions _defaultFormOptions = new FormOptions();

		// file signatures From (https://www.filesignatures.net/) for check on fileSignature ==> secure xss injection
		private static readonly Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
		{
			{ ".gif", new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
			{ ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
			{ ".jpeg", new List<byte[]>
				{
					new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
					new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
					new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
				}
			},
			{ ".jpg", new List<byte[]>
				{
					new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
					new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
					new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
				}
			},
			{ ".txt", new List<byte[]> { new byte[] { 0xEF, 0xBB, 0xBF } } },
			{ ".zip", new List<byte[]>
				{
					new byte[] { 0x50, 0x4B, 0x03, 0x04 },
					new byte[] { 0x50, 0x4B, 0x4C, 0x49, 0x54, 0x45 },
					new byte[] { 0x50, 0x4B, 0x53, 0x70, 0x58 },
					new byte[] { 0x50, 0x4B, 0x05, 0x06 },
					new byte[] { 0x50, 0x4B, 0x07, 0x08 },
					new byte[] { 0x57, 0x69, 0x6E, 0x5A, 0x69, 0x70 },
				}
			},
			{ ".pdf", new List<byte[]>
				{
					new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D }
				}
			},
			{ ".mp4", new List<byte[]>
				{
					new byte[] { 0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70 },
					new byte[] { 0x00, 0x00, 0x00, 0x20, 0x66, 0x74, 0x79, 0x70 }
				}
			},
			{ ".mp3", new List<byte[]>
				{
					new byte[] { 0x49, 0x44, 0x33 },
					new byte[] { 0xFF, 0xFB },
					new byte[] { 0xFF, 0xF3 },
					new byte[] { 0xFF, 0xF2 }
				}
			}
		};
		public static async Task<object> streamedOrBufferedProcess(FileUpload? formFiles, int? Id, IGenericrepo<Files> fileGenericRepo, HttpRequest Request)
		{
			List<IFormFile> bufferedFiles = new List<IFormFile>();
			object result = new List<byte[]>(); 
			if (formFiles != null)
			{
				foreach (var file in formFiles.FormFile)
				{
					if (file.Length <= _BufferedFileSizeLimite)
					{
						bufferedFiles.Add(file);
					}
				}

				if (bufferedFiles.Count > 0)
				{
					result = await BufferedProcessFormFile(bufferedFiles, formFiles.Note, Id, fileGenericRepo);
				}

				if (formFiles.FormFile.Any(file => file.Length > _BufferedFileSizeLimite))
				{
					result = await streamedProcessFormFile(Request);
				}
			}
			return result;
		}


		public static async Task<List<byte[]>> BufferedProcessFormFile(List<IFormFile>? formFiles, string fileNote, int? Id, IGenericrepo<Files> fileGenericRepo)
		{
			List<byte[]> fileData = new List<byte[]>();

			foreach (var File in formFiles)
			{
				#region get DisplayAttribute File Name       
				string fileDisplayName = string.Empty;
				var property =
				  typeof(FileUpload).GetProperty(File.Name.Substring(File.Name.IndexOf(".", StringComparison.Ordinal) + 1));

				if (property != null)
				{
					if (property.GetCustomAttribute(typeof(DisplayAttribute)) is DisplayAttribute displayAttribute)
					{
						fileDisplayName = $"{displayAttribute.Name}";
					}
				}
				#endregion

				#region Encode For Content  

				// Don't trust the file content sent by the client. ==> Secure when i make display 
				var trustedFileName = WebUtility.HtmlEncode(File.Name);
				#endregion

				#region size   Check    
				if (trustedFileName.Length == 0)
				{
					modelState.AddModelError(File.Name, $"{fileDisplayName}({trustedFileName}) is Empty");
					continue;
				}

				if (trustedFileName.Length > _BufferedFileSizeLimite)
				{
					var megaBytSiteLimite = _BufferedFileSizeLimite / 1048576;
					modelState.AddModelError(File.Name, $"{fileDisplayName}({trustedFileName}) is exceed {megaBytSiteLimite}MB");
					continue;
				}
				#endregion


				try
				{
					//Using Memory for Storge Data 
					using (var memoryStream = new MemoryStream())
					{
						//Read Data From Memory RAM 
						await File.CopyToAsync(memoryStream);

						#region After read data send it for check on ext and signature 
						if (memoryStream.Length == 0)
						{
							modelState.AddModelError(File.Name,
								$"{fileDisplayName}({trustedFileName}) is empty.");
							continue;
						}

						if (!IsValidFileExtensionAndSignature(File.FileName, memoryStream))
						{
							modelState.AddModelError(File.Name,
								$"{fileDisplayName}({trustedFileName}) file " +
								"type isn't permitted or the file's signature " +
								"doesn't match the file's extension.");
						}
						#endregion

						#region Saving file  
						else
						{
							//if (File.Length <= _BufferedFileSizeLimite)
							//	await SaveFileInPhysical(memoryStream.ToArray());
							//else
							await BufferedSaveFileInDb(memoryStream.ToArray(), File, Id, fileNote, fileGenericRepo);

							fileData.Add(memoryStream.ToArray());
						}

						#endregion

					}

				}
				catch (Exception ex)
				{
					modelState.AddModelError(File.Name,
					$"{fileDisplayName}({trustedFileName}) upload failed. " +
					$"Please contact the Help Desk for support. Error: {ex.HResult}");
				}
			}
			return fileData;

		}

	
		[DisableFormValueModelBinding]
		public static async Task<List<byte[]>> streamedProcessFormFile(HttpRequest Request)
		{
			List<byte[]> fileData = new List<byte[]>();

			if (!IsMultipartContentType(Request.ContentType))
			{
				modelState.AddModelError("File",
				   $"The request couldn't be processed");

			}
			var result = GetBoundry(MediaTypeHeaderValue.Parse(Request.ContentType), _defaultFormOptions.MultipartBoundaryLengthLimit);

			var reader = new MultipartReader(result, Request.Body);


			var section = await reader.ReadNextSectionAsync();
			while (section != null)
			{
				var hasContentDispositionHeader =
					   ContentDispositionHeaderValue.TryParse(
						   section.ContentDisposition, out var contentDisposition);

				if (hasContentDispositionHeader)
				{
					// This check assumes that there's a file
					// present without form data. If form data
					// is present, this method immediately fails
					if (!HasFileContentDisposition(contentDisposition))
					{
						modelState.AddModelError("File",
							$"The request couldn't be processed (Error 2).");

					}
					else
					{
						// Don't trust the file name sent by the client. To display
						// the file name, HTML-encode the value.
						//var trudtedFileNameForDisplay = WebUtility.HtmlEncode(contentDisposition.FileName.Value);


						try
						{
							using (var memoryStream = new MemoryStream())
							{
								await section.Body.CopyToAsync(memoryStream);

								// Check if the file is empty or exceeds the size limit.
								if (memoryStream.Length == 0)
								{
									modelState.AddModelError("File", "The file is empty.");
								}
								else if (memoryStream.Length > _StreemedFileSizeLimite)
								{
									var megabyteSizeLimit = _StreemedFileSizeLimite / 1048576;
									modelState.AddModelError("File",
									$"The file exceeds {megabyteSizeLimit:N1} MB.");
								}
								else if (!IsValidFileExtensionAndSignature(contentDisposition.FileName.Value, memoryStream))
								{
									modelState.AddModelError("File",
										"The file type isn't permitted or the file's " +
										"signature doesn't match the file's extension.");
								}

								else
								{
									#region Saving file  

									await SaveFileInPhysical(memoryStream.ToArray());

									fileData.Add(memoryStream.ToArray());
									#endregion

								}



							}
						}
						catch (Exception ex)
						{
							modelState.AddModelError("File",
								"The upload failed. Please contact the Help Desk " +
								$" for support. Error: {ex.HResult}");
						}
					}
				}
				// Drain any remaining section body that hasn't been consumed and
				// read the headers for the next section.
				section = await reader.ReadNextSectionAsync();

			}



			return fileData;

		}

		private static bool IsValidFileExtensionAndSignature(string fileName, Stream dataStream)
		{
			if (string.IsNullOrEmpty(fileName) || dataStream.Length == 0 || dataStream == null)
				return false;

			var ext = Path.GetExtension(fileName).ToLowerInvariant();

			if (string.IsNullOrEmpty(ext) || !_permittedExtensions.Contains(ext))
				return false;

			dataStream.Position = 0;
			using (var reader = new BinaryReader(dataStream))
			{
				if (reader.Equals(".txt"))
				{
					// Limits characters to ASCII encoding.
					if (_allowedChars.Length == 0)
					{
						for (int i = 0; i < dataStream.Length; i++)
						{
							if (reader.ReadByte() > byte.MaxValue)
							{
								return false;
							}
						}
					}
					// Limits characters to ASCII encoding and
					// values of the _allowedChars array.
					else
					{
						for (int i = 0; i < dataStream.Length; i++)
						{
							var result = reader.ReadByte();
							if (result > byte.MaxValue || !_allowedChars.Contains(result))
							{
								return false;
							}
						}

					}
					return true;
				}

				// File signature check
				// --------------------
				// With the file signatures provided in the _fileSignature
				// dictionary, the following code tests the input content's
				// file signature.
				var signatures = _fileSignature[ext];
				var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

				return signatures.Any(signature =>
					headerBytes.Take(signature.Length).SequenceEqual(signature));
			}

		}

		private static async Task SaveFileInPhysical(ReadOnlyMemory<byte> filecontent)
		{
			// For the file name of the uploaded file stored
			// server-side, use Path.GetRandomFileName to generate a safe
			// random file name.
			var trustedFileNameForSaving = Path.GetRandomFileName();
			var filePath = Path.Combine(_StordFilesPath, trustedFileNameForSaving);

			using (var fileStreem = File.Create(filePath))
			{
				await fileStreem.WriteAsync(filecontent);
			}

		}

		private static async Task BufferedSaveFileInDb(byte[]? filecontent, IFormFile? formFile,
		 int? Id, string? note, IGenericrepo<Files> fileGenericRepo)
		{

			var filData = new Files()
			{
				CourseId = Id,
				Content = filecontent,
				UntrustedName = formFile.FileName,
				UploadDT = DateTime.UtcNow,
				Note = note,
				Size = filecontent.Length

			};
			await fileGenericRepo.AddAsync(filData);
		}

		private static bool IsMultipartContentType(string contentType)
		{
			return !string.IsNullOrEmpty(contentType)
				&& contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
		}

		private static string GetBoundry(MediaTypeHeaderValue contentType, int BoundaryLengthLimit)
		{

			var result = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

			if (string.IsNullOrEmpty(result))
				throw new InvalidDataException("Missing content-type boundary.");

			if (result.Length > BoundaryLengthLimit)
				throw new InvalidDataException($"Multipart boundary length limit {BoundaryLengthLimit} exceeded.");

			return result;
		}

		private static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
		{
			return contentDisposition != null && contentDisposition.DispositionType.Equals("form-data");
				//&& !string.IsNullOrEmpty(contentDisposition.FileName.Value)
				//&& !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value);

		}
	}
}
