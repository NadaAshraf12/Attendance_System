using CleanArch.App.Services;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Repositories;
using MediatR;

namespace CleanArch.App.Features.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentCommand, ResponseModel>
    {
        private readonly IDepartmentRepository _repo;

        public UpdateDepartmentHandler(IDepartmentRepository repo) => _repo = repo;

        public async Task<ResponseModel> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var dept = await _repo.GetByIdWithChildrenAsync(request.Id);
            if (dept == null || dept.IsDeleted)
                return ResponseModel.Fail("Department not found");

            // -------- Update Department info --------
            if (!string.IsNullOrWhiteSpace(request.Name) && request.Name != "string" &&
                !string.Equals(dept.Name, request.Name, StringComparison.OrdinalIgnoreCase))
            {
                var other = await _repo.GetByNameAsync(request.Name);
                if (other != null && other.Id != request.Id)
                    return ResponseModel.Fail("Another department with the same name already exists");

                dept.Name = request.Name;
            }

            if (!string.IsNullOrWhiteSpace(request.Code) && request.Code != "string")
                dept.Code = request.Code;

            if (!string.IsNullOrWhiteSpace(request.Description) && request.Description != "string")
                dept.Description = request.Description;

            dept.IsActive = request.IsActive;
            dept.UpdatedAt = DateTime.UtcNow;
            dept.UpdatedOn = DateTime.UtcNow;

            // -------- Handle SubDepartments --------
            if (request.SubDepartments != null)
            {
                // existing children
                var existingChildren = dept.SubDepartments.ToList();

                // update or add
                foreach (var childDto in request.SubDepartments)
                {
                    if (childDto.Id.HasValue) // existing child → update
                    {
                        var child = existingChildren.FirstOrDefault(c => c.Id == childDto.Id.Value);
                        if (child != null)
                        {
                            child.Name = childDto.Name;
                            child.Code = childDto.Code;
                            child.Description = childDto.Description;
                            child.IsActive = childDto.IsActive;
                            child.UpdatedAt = DateTime.UtcNow;
                        }
                    }
                    else // new child → add
                    {
                        dept.SubDepartments.Add(new Department
                        {
                            Id = Guid.NewGuid(),
                            Name = childDto.Name,
                            Code = childDto.Code,
                            Description = childDto.Description,
                            IsActive = childDto.IsActive,
                            ParentDepartmentId = dept.Id,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }

                // remove any children not in request
                var idsInRequest = request.SubDepartments
                    .Where(x => x.Id.HasValue)
                    .Select(x => x.Id.Value)
                    .ToHashSet();

                foreach (var child in existingChildren)
                {
                    if (!idsInRequest.Contains(child.Id))
                    {
                        child.IsDeleted = true;
                        child.DeletedAt = DateTime.UtcNow;
                    }
                }
            }

            var updated = await _repo.UpdateAsync(dept);

            return ResponseModel.Success("Department updated successfully", new
            {
                updated.Id,
                updated.Name,
                updated.Code,
                updated.Description,
                updated.IsActive,
                SubDepartments = updated.SubDepartments.Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Code,
                    c.Description,
                    c.IsActive
                }).ToList()
            });
        }

    }


}
