using CleanArch.App.Services;
using CleanArch.App.Features.Departments;
using CleanArch.Domain.Repositories;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArch.App.Features.Departments.Queries.GetDepartmentById
{
    public class GetDepartmentByIdHandler : IRequestHandler<GetDepartmentByIdQuery, ResponseModel>
    {
        private readonly IDepartmentRepository _repo;

        public GetDepartmentByIdHandler(IDepartmentRepository repo) => _repo = repo;

        public async Task<ResponseModel> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var dept = await _repo.GetByIdWithChildrenAsync(request.Id);
            if (dept == null || dept.IsDeleted)
                return ResponseModel.Fail("Department not found");

            var dto = MapToDtoRecursive(dept);

            return ResponseModel.Success("Department retrieved", dto);
        }

        // ✅ Recursive mapping function
        private DepartmentDetailDto MapToDtoRecursive(Domain.Entities.Department dept)
        {
            return new DepartmentDetailDto
            {
                Id = dept.Id,
                Name = dept.Name,
                Code = dept.Code,
                Description = dept.Description,
                IsActive = dept.IsActive,
                ManagerId = dept.ManagerId,
                IsDeleted = dept.IsDeleted,
                Users = dept.Users?.Select(u => new DepartmentUserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email
                }).ToList() ?? new List<DepartmentUserDto>(),

                // Recursive children
                SubDepartments = dept.SubDepartments
                    .Where(sd => !sd.IsDeleted)
                    .Select(sd => MapToDtoRecursive(sd))
                    .ToList()
            };
        }
    }
}
