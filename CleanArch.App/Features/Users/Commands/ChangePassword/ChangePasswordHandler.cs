using CleanArch.App.Interface;
using CleanArch.App.Services;
using CleanArch.Infra.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArch.App.Features.Users.Commands.ChangePassword
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, ResponseModel>
    {
        private readonly ICurrentUserService _current;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IResponseModel _response;

        public ChangePasswordHandler(
            ICurrentUserService current,
            UserManager<ApplicationUser> userManager,
            IResponseModel response)
        {
            _current = current;
            _userManager = userManager;
            _response = response;
        }

        public async Task<ResponseModel> Handle(ChangePasswordCommand request, CancellationToken ct)
        {
            if (request.NewPassword != request.ConfirmNewPassword)
                throw new InvalidOperationException("New password and confirmation password do not match.");

            var userId = _current.UserId ?? throw new UnauthorizedAccessException();
            var user = await _userManager.FindByIdAsync(userId) ?? throw new KeyNotFoundException();

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join(" | ", result.Errors.Select(e => e.Description)));

            return _response.Response(200, false, "Password changed successfully.", new { userId });
        }

    }
}
