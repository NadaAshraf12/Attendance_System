namespace CleanArch.App.Services.Auth
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string SigningKey { get; set; } = default!;
        public int AccessTokenMinutes { get; set; } 
        public int RefreshTokenDays { get; set; }
    }
}
