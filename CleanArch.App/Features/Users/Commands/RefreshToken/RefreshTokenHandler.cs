using CleanArch.App.Interface;
using CleanArch.App.Interface.Auth;
using CleanArch.App.Services;
using CleanArch.Common.Dtos;
using MediatR;

namespace CleanArch.App.Features.Users.Commands.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, ResponseModel>
    {
        private readonly IJwtTokenService _jwt;
        private readonly IResponseModel _response;

        public RefreshTokenHandler(IJwtTokenService jwt, IResponseModel response)
        {
            _jwt = jwt;
            _response = response;
        }

        public async Task<ResponseModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            AuthResultDto refreshed = await _jwt.RefreshTokenAsync(request.RefreshToken, cancellationToken);
            return _response.Response(200, false, "Token refreshed successfully.", refreshed);
        }
    }
}
