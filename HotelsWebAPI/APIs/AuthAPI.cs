using HotelsWebAPI.Auth;
namespace HotelsWebAPI.APIs;

public class AuthApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/login", [AllowAnonymous] (HttpContext context, ITokenService tokenService, IUserRepository userRepository) =>
        {
            UserModel userModel = new()
            {
                UserName = context.Request.Query["username"],
                Password = context.Request.Query["password"]
            };
            var userDto = userRepository.GetUser(userModel);
            if (userDto == null) return Task.FromResult(Results.Unauthorized());
            var token = tokenService.BuildToken(app.Configuration["Jwt:Key"],
                app.Configuration["Jwt:Issuer"], userDto);
            return Task.FromResult(Results.Ok(token));
        });
    }
}