using CleanArch.App.Services;
using CleanArch.Domain.Repositories;
using CleanArch.Infra.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArch.App.Features.Departments.Commands.Assign_User_to_Department
{
    public class AssignUserToDepartmentHandler : IRequestHandler<AssignUserToDepartmentCommand, ResponseModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDepartmentRepository _deptRepo;

        public AssignUserToDepartmentHandler(UserManager<ApplicationUser> userManager, IDepartmentRepository deptRepo)
        {
            _userManager = userManager;
            _deptRepo = deptRepo;
        }

        public async Task<ResponseModel> Handle(AssignUserToDepartmentCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                return ResponseModel.Fail("User not found");

            var dept = await _deptRepo.GetByIdAsync(request.DepartmentId);
            if (dept == null)
                return ResponseModel.Fail("Department not found");

            user.DepartmentId = request.DepartmentId;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return ResponseModel.Fail(string.Join(";", result.Errors.Select(e => e.Description)));

            var deptPath = dept.Name;
            var parent = dept.ParentDepartment;
            while (parent != null)
            {
                deptPath = parent.Name + " > " + deptPath;
                parent = parent.ParentDepartment;
            }

            return ResponseModel.Success("User assigned successfully", new
            {
                UserId = user.Id,
                DepartmentId = dept.Id,
                DepartmentPath = deptPath
            });
        }
    }

}
