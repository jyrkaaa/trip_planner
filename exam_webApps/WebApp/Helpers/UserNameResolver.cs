using Base.Contracts;
using Base.Helpers;

namespace WebApp.Helpers;

public class UserNameResolver : IUserNameResolver
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserNameResolver(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string CurrentUserName => _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "system";
    public string CurrentUserId => _httpContextAccessor.HttpContext?.User.GetUserIdSafe()?.ToString() ?? "system";
}
