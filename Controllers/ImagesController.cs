using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jaahub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly long _maxFileSize = 10 * 1024 * 1024; // 10MB
        private static readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
        private const string UploadFolder = "uploads"; // زیر wwwroot

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            if (file.Length > _maxFileSize)
                return BadRequest("File too large. Max 10MB.");

            // بررسی کانتنت‌تایپ و پسوند
            var contentType = file.ContentType?.ToLowerInvariant() ?? "";
            if (!contentType.StartsWith("image/"))
                return BadRequest("Only image files are allowed.");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(ext))
                return BadRequest("Allowed extensions: .jpg, .jpeg, .png, .webp");

            // مسیر ذخیره
            var wwwroot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadPath = Path.Combine(wwwroot, UploadFolder);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // نام یکتا
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(uploadPath, fileName);

            // ذخیره فایل
            await using (var stream = System.IO.File.Create(fullPath))
            {
                await file.CopyToAsync(stream);
            }

            // URL عمومی فایل
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var url = $"{baseUrl}/{UploadFolder}/{fileName}";

            return Ok(new
            {
                fileName,
                url,
                size = file.Length,
                contentType = file.ContentType
            });
        }

        [HttpGet("{fileName}")]
        [Produces("application/json")]
        public IActionResult GetInfo(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(fileName) || !_allowedExtensions.Contains(ext))
                return BadRequest("Invalid file name.");

            var wwwroot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var fullPath = Path.Combine(wwwroot, UploadFolder, fileName);

            if (!System.IO.File.Exists(fullPath))
                return NotFound();

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var url = $"{baseUrl}/{UploadFolder}/{fileName}";

            var info = new FileInfo(fullPath);
            return Ok(new
            {
                fileName,
                url,
                size = info.Length,
                createdAt = info.CreationTimeUtc
            });
        }
    }
}
