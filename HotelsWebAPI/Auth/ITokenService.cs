namespace HotelsWebAPI.Auth;

public interface ITokenService
{
    string BuildToken(string key, string issuer, UserDto user);
}    