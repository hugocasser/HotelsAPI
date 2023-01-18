namespace HotelsWebAPI.Auth;

public class UserRepository : IUserRepository
{
    private List<UserDto> _users => new()
    {
        new UserDto("Josh", "qwerty123"),
        new UserDto("j", "1")
    };

    public UserDto GetUser(UserModel userModel) =>
        _users.FirstOrDefault(u =>
            string.Equals(u.UserName, userModel.UserName) &&
            string.Equals(u.Password, userModel.Password)) ??
        throw new Exception();
}            