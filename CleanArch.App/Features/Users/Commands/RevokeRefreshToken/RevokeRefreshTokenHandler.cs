using CleanArch.App.Interface;
using CleanArch.App.Interface.Auth;
using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Users.Commands.RevokeRefreshToken
{
    public class RevokeRefreshTokenHandler : IRequestHandler<RevokeRefreshTokenCommand, ResponseModel>
    {
        private readonly IJwtTokenService _jwt;
        private readonly IResponseModel _response;

        public RevokeRefreshTokenHandler(IJwtTokenService jwt, IResponseModel response)
        {
            _jwt = jwt;
            _response = response;
        }

        public async Task<ResponseModel> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            await _jwt.RevokeTokenAsync(request.RefreshToken, cancellationToken);
            return _response.Response(200, false, "Refresh token revoked.", new { revoked = true });
        }
    }
}
