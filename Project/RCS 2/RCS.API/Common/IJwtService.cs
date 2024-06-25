namespace RCS.API.Common;

public interface IJwtService
{
    string Email { get; }
    bool IsAdmin { get; }
}