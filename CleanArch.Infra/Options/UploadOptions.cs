namespace CleanArch.Infra.Options
{

    public class UploadOptions
    {
        public string BaseFolder { get; set; } = "Uploads"; // in wwwroot
        public string[] AllowedExtensions { get; set; } = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        public int MaxFileSizeMB { get; set; } = 5;
        public string? BaseUrlOverride { get; set; } 
    }
}
