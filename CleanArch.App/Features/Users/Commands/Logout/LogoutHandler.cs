using CleanArch.App.Interface;
using CleanArch.App.Services;
using CleanArch.Infra.Data;
using CleanArch.Infra.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArch.App.Features.Users.Commands.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutCommand, ResponseModel>
    {
        private readonly ICurrentUserService _current;
        private readonly ApplicationDbContext _db;

        public LogoutHandler(ICurrentUserService current, ApplicationDbContext db)
        {
            _current = current;
            _db = db;
        }

        public async Task<ResponseModel> Handle(LogoutCommand request, CancellationToken ct)
        {
            var userId = _current.UserId;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                return ResponseModel.Fail("Unauthorized", 401);

            if (request.AllDevices)
            {
                var tokens = await _db.RefreshTokens
                    .Where(t => t.UserId == userGuid && t.RevokedOn == null) // ✅ استخدم العمود
                    .ToListAsync(ct);

                foreach (var t in tokens)
                {
                    t.RevokedOn = DateTime.UtcNow;
                }

                await _db.SaveChangesAsync(ct);
                return ResponseModel.Success("Logged out from all devices");
            }
            else
            {
                var token = await _db.RefreshTokens
                    .FirstOrDefaultAsync(t => t.Token == request.RefreshToken, ct);

                if (token is null || token.UserId != userGuid)
                    return ResponseModel.Fail("Invalid refresh token", 400);

                if (token.RevokedOn == null) // ✅ بدل !token.IsRevoked
                {
                    token.RevokedOn = DateTime.UtcNow;
                    await _db.SaveChangesAsync(ct);
                }

                return ResponseModel.Success("Logged out");
            }
        }
    }
}
