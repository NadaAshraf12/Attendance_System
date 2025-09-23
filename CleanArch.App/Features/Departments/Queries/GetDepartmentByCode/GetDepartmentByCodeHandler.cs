using CleanArch.App.Services;
using CleanArch.Domain.Repositories;
using MediatR;

namespace CleanArch.App.Features.Departments.Queries.GetDepartmentByCode
{
    public class GetDepartmentByCodeHandler : IRequestHandler<GetDepartmentByCodeQuery, ResponseModel>
    {
        private readonly IDepartmentRepository _repo;

        public GetDepartmentByCodeHandler(IDepartmentRepository repo) => _repo = repo;

        public async Task<ResponseModel> Handle(GetDepartmentByCodeQuery request, CancellationToken cancellationToken)
        {
            var dept = await _repo.GetByCodeWithChildrenAsync(request.Code);
            if (dept == null)
                return ResponseModel.Fail("Department not found");

            var result = new
            {
                dept.Id,
                dept.Name,
                dept.Code,
                dept.Description,
                dept.IsActive,
                SubDepartments = dept.SubDepartments
                    .Where(sd => !sd.IsDeleted)
                    .Select(sd => new
                    {
                        sd.Id,
                        sd.Name,
                        sd.Code,
                        sd.Description,
                        sd.IsActive
                    }).ToList()
            };

            return ResponseModel.Success("Department fetched successfully", result);
        }
    }
}
