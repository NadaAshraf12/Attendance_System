// Application/Features/Roles/Commands/DeleteRole/DeleteRoleHandler.cs
using CleanArch.App.Services;
using CleanArch.Common.Dtos;
using CleanArch.Infra.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArch.App.Features.Roles.Commands.DeleteRole
{
    public class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, ResponseModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private const string DefaultRole = "User";

        public DeleteRoleHandler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseModel> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByNameAsync(request.RoleName);
            if (role is null)
                return ResponseModel.Fail("Role not found");

            // هات كل اليوزرز اللي واخدين الـ role
            var users = await _userManager.GetUsersInRoleAsync(request.RoleName);
            foreach (var user in users)
            {
                await _userManager.RemoveFromRoleAsync(user, request.RoleName);

                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Any())
                {
                    if (!await _roleManager.RoleExistsAsync(DefaultRole))
                        await _roleManager.CreateAsync(new ApplicationRole(DefaultRole));

                    await _userManager.AddToRoleAsync(user, DefaultRole);
                }
            }

            // احذف الـ role نفسه
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                return ResponseModel.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));

            return ResponseModel.Success("Role deleted successfully");
        }
    }
}
