using CleanArch.App.Services;
using CleanArch.Domain.Repositories;
using MediatR;

namespace CleanArch.App.Features.Departments.Commands.DeleteDepartment
{
    public class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentCommand, ResponseModel>
    {
        private readonly IDepartmentRepository _repo;

        public DeleteDepartmentHandler(IDepartmentRepository repo) => _repo = repo;

        public async Task<ResponseModel> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _repo.GetByIdWithChildrenAsync(request.Id); // 👈 مهم نجيب SubDepartments
            if (department == null)
                return ResponseModel.Fail("Department not found");

            // ✅ Cascade Soft Delete
            await SoftDeleteRecursive(department);

            return ResponseModel.Success("Department and its sub-departments soft-deleted successfully");
        }

        private async Task SoftDeleteRecursive(CleanArch.Domain.Entities.Department dept)
        {
            dept.IsDeleted = true;
            dept.DeletedAt = DateTime.UtcNow;

            if (dept.SubDepartments != null && dept.SubDepartments.Any())
            {
                foreach (var child in dept.SubDepartments)
                {
                    await SoftDeleteRecursive(child);
                }
            }

            await _repo.UpdateAsync(dept);
        }
    }
}
