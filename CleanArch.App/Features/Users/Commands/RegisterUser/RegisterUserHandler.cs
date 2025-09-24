using CleanArch.App.Interface;
using CleanArch.App.Interface.Auth;
using CleanArch.App.Resources;
using CleanArch.App.Services;
using CleanArch.Common.Dtos;
using CleanArch.Domain.Repositories;
using CleanArch.Infra.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace CleanArch.App.Features.Users.Commands.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, ResponseModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwt;
        private readonly IResponseModel _response;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDepartmentRepository _departmentRepository;

        public RegisterUserHandler(
            UserManager<ApplicationUser> userManager,
            IJwtTokenService jwt,
            IResponseModel response,
            IStringLocalizer<SharedResource> localizer,
            IDepartmentRepository departmentRepository)
        {
            _userManager = userManager;
            _jwt = jwt;
            _response = response;
            _localizer = localizer;
            _departmentRepository = departmentRepository;
        }

        public async Task<ResponseModel> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetByCodeAsync(request.DepartmentCode);
            if (department == null || department.IsDeleted)
            {
                return _response.Response(404, true, $"Department/SubDepartment with code {request.DepartmentCode} not found", null);
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return _response.Response(400, true, _localizer["EmailAlreadyExists"], null);
            }

            var user = new ApplicationUser
            {
                UserName = request.UserName ?? request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                FullName = request.FirstName + " " + request.LastName,
                DepartmentId = department.Id
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join(" | ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "User");

            AuthResultDto tokens = await _jwt.CreateTokenAsync(user, cancellationToken);

            var data = new
            {
                user = new
                {
                    user.Id,
                    user.Email,
                    user.FullName,
                    Department = department.Name,
                    DepartmentCode = department.Code,
                    ParentDepartment = department.ParentDepartment != null
                        ? new { department.ParentDepartment.Id, department.ParentDepartment.Name, department.ParentDepartment.Code }
                        : null
                },
                tokens
            };

            return _response.Response(200, false, _localizer["UserRegistered"], data);
        }
    }
}
