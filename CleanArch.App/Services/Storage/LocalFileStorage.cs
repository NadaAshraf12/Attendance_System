using CleanArch.App.Interface.Storage;
using CleanArch.Common.Dtos;
using CleanArch.Infra.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.App.Services.Storage
{
    public class LocalFileStorage :IFileStorage
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _http;
        private readonly UploadOptions _opt;


        public LocalFileStorage(IWebHostEnvironment env, IHttpContextAccessor http, IOptions<UploadOptions> opt)
        {
            _env = env;
            _http = http;
            _opt = opt.Value;
        }

        public async Task<FileResourceDto?> SaveAsync(Stream content, string originalFileName, string folder, CancellationToken ct = default)
        {
            if (content == null || content == Stream.Null)
                return null;


            var ext = Path.GetExtension(originalFileName).ToLowerInvariant();
            if (!_opt.AllowedExtensions.Contains(ext))
                throw new InvalidOperationException($"File type '{ext}' is not allowed.");


            // Optional: الحجم لو متوفّر (لو Stream ملف، غالبًا مش هتعرف قبل ما تقراه)
            if (content is MemoryStream ms && ms.Length > _opt.MaxFileSizeMB * 1024L * 1024L)
                throw new InvalidOperationException($"Max file size is {_opt.MaxFileSizeMB} MB.");


            var basePhysical = Path.Combine(_env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot"), _opt.BaseFolder, folder);
            Directory.CreateDirectory(basePhysical);


            var safeFileName = $"{Guid.NewGuid():N}{ext}";
            var physicalPath = Path.Combine(basePhysical, safeFileName);


            // حفظ فعلي
            using (var fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await content.CopyToAsync(fs, ct);
            }


            var baseUrl = BuildBaseUrl();
            var url = CombineUrl(baseUrl, _opt.BaseFolder, folder, safeFileName);


            return new FileResourceDto
            {
                Url = url,
                FileName = Path.GetFileName(originalFileName),
                ContentType = TryGuessContentType(ext),
                SizeBytes = TryGetFileSize(physicalPath)
            };
        }

        public async Task<IReadOnlyList<FileResourceDto>> SaveManyAsync(IEnumerable<(Stream Content, string OriginalFileName)> files, string folder, CancellationToken ct = default)
        {
            var list = new List<FileResourceDto>();
            foreach (var (content, name) in files)
            {
                var saved = await SaveAsync(content, name, folder, ct);
                if (saved != null) list.Add(saved);
            }
            return list;
        }


        public Task<bool> DeleteByUrlAsync(string fileUrl, CancellationToken ct = default)
        {
            try
            {
                var decoded = WebUtility.UrlDecode(fileUrl);
                if (!Uri.TryCreate(decoded, UriKind.Absolute, out var uri)) return Task.FromResult(false);


                var localPath = uri.LocalPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
                var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
                var fullPath = Path.Combine(webRoot, localPath);


                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        private string BuildBaseUrl()
        {
            if (!string.IsNullOrWhiteSpace(_opt.BaseUrlOverride))
                return _opt.BaseUrlOverride.TrimEnd('/');


            var req = _http.HttpContext?.Request;
            if (req == null) return string.Empty; // fallback: empty => relative URLs
            var scheme = req.Scheme;
            var host = req.Host.Value;
            return $"{scheme}://{host}";
        }


        private static string CombineUrl(params string[] parts)
        => string.Join('/', parts.Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p.Trim('/')));


        private static string? TryGuessContentType(string ext)
        {
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".webp" => "image/webp",
                ".pdf" => "application/pdf",
                _ => null
            };
        }


        private static long TryGetFileSize(string physicalPath)
        => File.Exists(physicalPath) ? new FileInfo(physicalPath).Length : 0L;

    }
}
