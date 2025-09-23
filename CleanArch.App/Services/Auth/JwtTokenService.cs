using CleanArch.App.Interface.Auth;
using CleanArch.Common.Dtos;
using CleanArch.Infra.Data;
using CleanArch.Infra.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArch.App.Services.Auth
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly JwtOptions _options;

        public JwtTokenService(UserManager<ApplicationUser> userManager,
                               ApplicationDbContext db,
                               IOptions<JwtOptions> options)
        {
            _userManager = userManager;
            _db = db;
            _options = options.Value;
        }

        public async Task<AuthResultDto> CreateTokenAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new(ClaimTypes.Name, user.UserName ?? ""),
        };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(_options.AccessTokenMinutes);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refresh = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                ExpiresOn = DateTime.UtcNow.AddDays(_options.RefreshTokenDays),
            };

            _db.RefreshTokens.Add(refresh);
            await _db.SaveChangesAsync(cancellationToken);

            return new AuthResultDto
            {
                AccessToken = accessToken,
                ExpiresAtUtc = expires,
                RefreshToken = refresh.Token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    EmailConfirmed = user.EmailConfirmed,
                    IsDeleted = user.IsDeleted,
                    Roles = roles
                }
            };
        }

        public async Task<AuthResultDto> CreateTokenAsync(
            ApplicationUser user,
            string? device = null,
            string? deviceId = null,
            string? ipAddress = null,
            CancellationToken cancellationToken = default)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
        new(ClaimTypes.Name, user.UserName ?? "")
    };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(_options.AccessTokenMinutes);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refresh = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                ExpiresOn = DateTime.UtcNow.AddDays(_options.RefreshTokenDays),
                Device = device,
                DeviceId = deviceId,
                IpAddress = ipAddress
            };

            _db.RefreshTokens.Add(refresh);
            await _db.SaveChangesAsync(cancellationToken);

            return new AuthResultDto
            {
                AccessToken = accessToken,
                ExpiresAtUtc = expires,
                RefreshToken = refresh.Token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    EmailConfirmed = user.EmailConfirmed,
                    IsDeleted = user.IsDeleted,
                    Roles = roles
                }
            };
        }
        public async Task<AuthResultDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var rt = await _db.RefreshTokens
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Token == refreshToken, cancellationToken)
                ?? throw new UnauthorizedAccessException("Invalid refresh token");

            if (rt.IsRevoked || rt.ExpiresOn <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token expired or revoked");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == rt.UserId, cancellationToken)
                       ?? throw new UnauthorizedAccessException("User not found");

            rt.RevokedOn = DateTime.UtcNow;

            var result = await CreateTokenAsync(user, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var rt = await _db.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken, cancellationToken);

            if (rt is null) return;

            rt.RevokedOn = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);
        }

    }
}
