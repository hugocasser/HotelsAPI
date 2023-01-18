namespace HotelsWebAPI.Auth;

public interface IUserRepository
{
    UserDto GetUser(UserModel userModel);
}