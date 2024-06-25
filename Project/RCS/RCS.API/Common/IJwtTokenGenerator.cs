namespace RCS.API.Common;

public interface IJwtTokenGenerator
{
    string GenerateToken(string login, string password);
}