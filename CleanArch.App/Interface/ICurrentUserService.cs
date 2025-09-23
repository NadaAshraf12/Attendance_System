using System.Security.Claims;

namespace CleanArch.App.Interface
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? UserEmail { get; }
        ClaimsPrincipal Principal { get; }
    }
}
