using CleanArch.App.Interface;
using CleanArch.App.Services;
using CleanArch.Infra.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArch.App.Features.Users.Commands.SoftDeleteUser
{
    public class SoftDeleteUserHandler : IRequestHandler<SoftDeleteUserCommand, ResponseModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IResponseModel _response;

        public SoftDeleteUserHandler(UserManager<ApplicationUser> userManager, IResponseModel response)
        {
            _userManager = userManager;
            _response = response;
        }

        public async Task<ResponseModel> Handle(SoftDeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId) 
                       ?? throw new KeyNotFoundException();

            user.IsDeleted = request.IsDeleted;
            var res = await _userManager.UpdateAsync(user);

            if (!res.Succeeded)
                throw new InvalidOperationException(string.Join(" | ", res.Errors.Select(e => e.Description)));

            var data = new { userId = user.Id, isDeleted = user.IsDeleted };
            string message = user.IsDeleted ? "User soft-deleted." : "User restored.";

            return _response.Response(200, false, message, data);
        }
    }
}
