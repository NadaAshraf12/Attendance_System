using CleanArch.App.Interface;
using CleanArch.App.Services;
using CleanArch.Common.Dtos;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Repositories;
using CleanArch.Infra.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArch.App.Features.Users.Queries.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, ResponseModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IResponseModel _responseModel;
        private readonly IDepartmentRepository _departmentRepository;

        public GetUserByIdHandler(
            UserManager<ApplicationUser> userManager,
            IResponseModel responseModel,
            IDepartmentRepository departmentRepository)
        {
            _userManager = userManager;
            _responseModel = responseModel;
            _departmentRepository = departmentRepository;
        }

        public async Task<ResponseModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId)
                       ?? throw new KeyNotFoundException("User not found");

            var roles = await _userManager.GetRolesAsync(user);

            Department? department = null;
            if (user.DepartmentId != null)
                department = await _departmentRepository.GetByIdWithChildrenAsync(user.DepartmentId.Value);

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                IsDeleted = user.IsDeleted,
                Roles = roles,
                Department = department == null ? null : MapToDepartmentDto(department)
            };

            return _responseModel.Response(200, false, "User retrieved successfully", userDto);
        }

        private CleanArch.Common.Dtos.DepartmentDto MapToDepartmentDto(CleanArch.Domain.Entities.Department dept)
        {
            if (dept == null) return null!; 

            var dto = new CleanArch.Common.Dtos.DepartmentDto
            {
                Id = dept.Id,
                Name = dept.Name,
                Code = dept.Code,
                SubDepartments = dept.SubDepartments?
                    .Where(sd => !sd.IsDeleted) 
                    .Select(sd => MapToDepartmentDto(sd))
                    .ToList() ?? new List<CleanArch.Common.Dtos.DepartmentDto>()
            };

            return dto;
        }

    }
}
