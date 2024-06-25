using System.Security.Claims;

namespace RCS.API.Common;

public class JwtService(IHttpContextAccessor _httpContextAccessor) : IJwtService
{
    public string Email => _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
    public bool IsAdmin => _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value == "Admin";
}