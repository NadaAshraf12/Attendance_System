using CleanArch.App.Interface;
using CleanArch.App.Services;
using CleanArch.Common.Dtos;
using CleanArch.Domain.Repositories;
using CleanArch.Infra.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.App.Features.Users.Queries.ListUsers
{
    public class ListUsersHandler : IRequestHandler<ListUsersQuery, ResponseModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IResponseModel _responseModel;
        private readonly IDepartmentRepository _departmentRepository;

        public ListUsersHandler(
            UserManager<ApplicationUser> userManager,
            IResponseModel responseModel,
            IDepartmentRepository departmentRepository)
        {
            _userManager = userManager;
            _responseModel = responseModel;
            _departmentRepository = departmentRepository;
        }

        public async Task<ResponseModel> Handle(ListUsersQuery request, CancellationToken cancellationToken)
        {
            var q = _userManager.Users.AsQueryable();

            if (!request.IncludeDeleted)
                q = q.Where(u => !u.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.Search))
                q = q.Where(u => (u.Email != null && u.Email.Contains(request.Search))
                          || (u.UserName != null && u.UserName.Contains(request.Search)));

            var total = await q.LongCountAsync(cancellationToken);

            var users = await q.OrderBy(u => u.Email)
                               .Skip((request.Page - 1) * request.PageSize)
                               .Take(request.PageSize)
                               .ToListAsync(cancellationToken);

            var deptIds = users.Where(u => u.DepartmentId.HasValue)
                               .Select(u => u.DepartmentId!.Value)
                               .Distinct()
                               .ToList();

            var depts = new Dictionary<Guid, DepartmentDto>();
            if (deptIds.Any())
            {
                var deptEntities = await _departmentRepository.GetByIdsAsync(deptIds); 
                depts = deptEntities.ToDictionary(
                    d => d.Id,
                    d => new DepartmentDto { Id = d.Id, Name = d.Name, Code = d.Code }
                );
            }

            var result = new List<UserDto>(users.Count);
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                result.Add(new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserName = u.UserName,
                    PhoneNumber = u.PhoneNumber,
                    EmailConfirmed = u.EmailConfirmed,
                    IsDeleted = u.IsDeleted,
                    Roles = roles,
                    Department = u.DepartmentId.HasValue && depts.ContainsKey(u.DepartmentId.Value)
                        ? depts[u.DepartmentId.Value]
                        : null
                });
            }

            var pagedResult = new PagedResult<UserDto>
            {
                Items = result,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = total
            };

            return _responseModel.Response(200, false, "Users retrieved successfully", pagedResult);
        }
    }
}
