using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Users.Queries.GetUserByEmail
{
    public record GetUserByEmailQuery(string Email) : IRequest<ResponseModel>;
}