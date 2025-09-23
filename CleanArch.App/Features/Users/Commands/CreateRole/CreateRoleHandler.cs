using CleanArch.App.Services;
using CleanArch.Common.Dtos;
using CleanArch.Infra.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; // مهم عشان ToListAsync / MaxAsync
using System.Linq;

namespace CleanArch.App.Features.Roles.Commands.CreateRole
{
    public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, ResponseModel>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public CreateRoleHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ResponseModel> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            // Check if role exists
            if (await _roleManager.RoleExistsAsync(request.RoleName))
            {
                return ResponseModel.Fail($"Role '{request.RoleName}' already exists.");
            }

            // Get all roles to calculate next RoleType
            var roles = await _roleManager.Roles.ToListAsync(cancellationToken);

            int nextRoleValue = 0;
            if (roles.Any())
            {
                // هنجيب أكبر رقم موجود ونزود عليه واحد
                nextRoleValue = roles.Max(r => (int)r.RoleType) + 1;
            }

            var newRole = new ApplicationRole(request.RoleName)
            {
                RoleType = (CleanArch.Domain.Enums.RoleType)nextRoleValue,
                Description = request.Description // لو بعتها في الـ Command
            };

            var result = await _roleManager.CreateAsync(newRole);

            if (result.Succeeded)
            {
                return ResponseModel.Success($"Role '{request.RoleName}' created successfully with RoleType {nextRoleValue}.");
            }

            return ResponseModel.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
