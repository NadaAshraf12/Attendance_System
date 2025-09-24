using CleanArch.App.Interface;
using CleanArch.App.Services;
using CleanArch.Infra.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArch.App.Features.Users.Commands.ChangeEmail
{
    public class ChangeEmailHandler : IRequestHandler<ChangeEmailCommand, ResponseModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IResponseModel _response;

        public ChangeEmailHandler(UserManager<ApplicationUser> userManager, IResponseModel response)
        {
            _userManager = userManager;
            _response = response;
        }

        public async Task<ResponseModel> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.CurrentEmail)
                       ?? throw new KeyNotFoundException("User not found");

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, request.NewEmail);

            var result = await _userManager.ChangeEmailAsync(user, request.NewEmail, token);

            if (!result.Succeeded)
                return _response.Response(400, true, "Failed to change email.", null);

            return _response.Response(200, false, "Email changed successfully.", null);
        }
    }
}
