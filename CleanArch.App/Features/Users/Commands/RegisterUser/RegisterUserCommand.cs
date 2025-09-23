using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Users.Commands.RegisterUser
{
    public record RegisterUserCommand(
    string Email,
    string Password,
    string? UserName,
    string FirstName,
    string LastName,
    string DepartmentCode   
) : IRequest<ResponseModel>;

}
