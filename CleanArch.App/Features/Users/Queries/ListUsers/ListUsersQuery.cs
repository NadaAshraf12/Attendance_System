using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Users.Queries.ListUsers
{
    public record ListUsersQuery(
        int Page = 1,
        int PageSize = 20,
        string? Search = null,
        bool IncludeDeleted = false
    ) : IRequest<ResponseModel>;
}