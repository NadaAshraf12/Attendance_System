using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(string UserId) : IRequest<ResponseModel>;
}