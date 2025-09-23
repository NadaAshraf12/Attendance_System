// ResetPasswordHandler.cs
using CleanArch.App.Services;
using CleanArch.Infra.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArch.App.Features.Users.Commands.ResetPassword
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, ResponseModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResponseModel> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return ResponseModel.Fail("Invalid email");

            // Important: Decode the token (جاي URL encoded من الميل)
            var decodedToken = request.Token.Replace(" ", "+");

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return ResponseModel.Fail(errors);
            }

            return ResponseModel.Success("Password reset successfully");
        }
    }
}
