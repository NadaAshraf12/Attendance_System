using CleanArch.App.Features.Departments;
using CleanArch.App.Interface;
using CleanArch.App.Services;
using CleanArch.Common.Dtos;
using CleanArch.Domain.Repositories;
using CleanArch.Infra.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArch.App.Features.Users.Queries.GetUserByEmail
{
    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailQuery, ResponseModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IResponseModel _responseModel;
        private readonly IDepartmentRepository _departmentRepository;

        public GetUserByEmailHandler(
            UserManager<ApplicationUser> userManager,
            IResponseModel responseModel,
            IDepartmentRepository departmentRepository)
        {
            _userManager = userManager;
            _responseModel = responseModel;
            _departmentRepository = departmentRepository;
        }

        public async Task<ResponseModel> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email)
                       ?? throw new KeyNotFoundException("User not found");

            var roles = await _userManager.GetRolesAsync(user);

            var department = user.DepartmentId != null
                ? await _departmentRepository.GetByIdWithChildrenAsync(user.DepartmentId.Value)
                : null;

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                IsDeleted = user.IsDeleted,
                Roles = roles,
                Department = department == null ? null : MapDepartmentRecursive(department)
            };

            return _responseModel.Response(200, false, "User retrieved successfully", userDto);
        }

        private Common.Dtos.DepartmentDto MapDepartmentRecursive(CleanArch.Domain.Entities.Department dept)
        {
            return new Common.Dtos.DepartmentDto
            {
                Id = dept.Id,
                Name = dept.Name,
                Code = dept.Code,
                SubDepartments = dept.SubDepartments?.Select(MapDepartmentRecursive).ToList() ?? new List<Common.Dtos.DepartmentDto>()
            };
        }
    }
}
