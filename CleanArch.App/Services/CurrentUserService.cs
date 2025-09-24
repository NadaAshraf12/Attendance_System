using CleanArch.App.Interface;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CleanArch.Infra.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _http;

    public CurrentUserService(IHttpContextAccessor http) => _http = http;

    public string? UserId => _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                           ?? _http.HttpContext?.User?.FindFirstValue(ClaimTypes.Name) 
                           ?? _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? UserEmail => _http.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public ClaimsPrincipal Principal => _http.HttpContext?.User ?? new ClaimsPrincipal();
}
