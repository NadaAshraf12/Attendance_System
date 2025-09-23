using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Users.Commands.SoftDeleteUser
{
    public record SoftDeleteUserCommand(string UserId, bool IsDeleted) : IRequest<ResponseModel>;
}
