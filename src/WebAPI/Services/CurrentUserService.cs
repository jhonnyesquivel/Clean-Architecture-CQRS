using System.Security.Claims;

using Zemoga_Test.Application.Common.Interfaces;

namespace Zemoga_Test.WebAPI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? UserRole => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
}
