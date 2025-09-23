using CleanArch.Common.Dtos;
using CleanArch.Infra.Identity;

namespace CleanArch.App.Interface.Auth
{
    public interface IJwtTokenService
    {
        Task<AuthResultDto> CreateTokenAsync(ApplicationUser user, CancellationToken cancellationToken = default);
        Task<AuthResultDto> CreateTokenAsync(
            ApplicationUser user,
            string? device = null,
            string? deviceId = null,
            string? ipAddress = null,
            CancellationToken cancellationToken = default);
        Task<AuthResultDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }

}
