namespace CleanArch.Common.Dtos
{
    public class AuthResultDto
    {
        public string AccessToken { get; set; } = default!;
        public DateTime ExpiresAtUtc { get; set; }
        public string RefreshToken { get; set; } = default!;
        public UserDto User { get; set; } = default!;
    }
}
