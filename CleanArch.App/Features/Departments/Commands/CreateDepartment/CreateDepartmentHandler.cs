using CleanArch.App.Services;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Repositories;
using MediatR;

namespace CleanArch.App.Features.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentHandler : IRequestHandler<CreateDepartmentCommand, ResponseModel>
    {
        private readonly IDepartmentRepository _repo;

        public CreateDepartmentHandler(IDepartmentRepository repo) => _repo = repo;

        public async Task<ResponseModel> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            // ✅ check duplicate
            var exists = await _repo.GetByNameAsync(request.Name);
            if (exists != null)
                return ResponseModel.Fail("Department already exists");

            // ✅ if found Parent Department
            Department? parent = null;
            if (request.ParentDepartmentId.HasValue)
            {
                parent = await _repo.GetByIdAsync(request.ParentDepartmentId.Value);
                if (parent == null)
                    return ResponseModel.Fail("Parent department not found");
            }

            var dept = new Department
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Code = request.Code,
                Description = request.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedOn = DateTime.UtcNow,

                // ✅ Self Reference
                ParentDepartmentId = parent?.Id
            };

            await _repo.AddAsync(dept);

            // ✅ Build Path (if found Parent)
            var deptPath = dept.Name;
            while (parent != null)
            {
                deptPath = parent.Name + " > " + deptPath;
                parent = parent.ParentDepartment;
            }

            return ResponseModel.Success("Department created successfully", new
            {
                dept.Id,
                dept.Name,
                dept.Code,
                DepartmentPath = deptPath
            });
        }
    }
}
