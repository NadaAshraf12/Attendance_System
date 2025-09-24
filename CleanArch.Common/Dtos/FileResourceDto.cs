namespace CleanArch.Common.Dtos
{
    public class FileResourceDto
    {
        public required string Url { get; set; }
        public required string FileName { get; set; }
        public string? ContentType { get; set; }
        public long SizeBytes { get; set; }
    }
}
