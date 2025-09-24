using CleanArch.App.Interface;
using CleanArch.App.Services;
using CleanArch.Infra.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArch.App.Features.Users.Commands.AssignRoles
{
    public class AssignRolesHandler : IRequestHandler<AssignRolesCommand, ResponseModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IResponseModel _response;

        public AssignRolesHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IResponseModel response)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _response = response;
        }

        public async Task<ResponseModel> Handle(AssignRolesCommand request, CancellationToken ct)
        {
            var user = await _userManager.FindByIdAsync(request.UserId)
                       ?? throw new KeyNotFoundException("User not found.");

            var current = await _userManager.GetRolesAsync(user);

            var validRoles = new List<string>();
            foreach (var role in request.Roles.Distinct())
            {
                if (await _roleManager.RoleExistsAsync(role))
                    validRoles.Add(role);
            }

            if (!validRoles.Any())
                return _response.Response(400, true, "No valid roles found to assign.", null);

            await _userManager.RemoveFromRolesAsync(user, current);
            await _userManager.AddToRolesAsync(user, validRoles);

            var result = new
            {
                userId = user.Id,
                assignedRoles = validRoles.ToArray()
            };

            return _response.Response(200, false, "Roles assigned successfully.", result);
        }
    }
}
