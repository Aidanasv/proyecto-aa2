namespace Services;

using Models;
public interface IAuthService
{
    public string Login(UserAuth user);
    public Task<string> Register(UserRegisterPassword user, string role);
    public string GenerateToken(UserToken userToken);

    


}