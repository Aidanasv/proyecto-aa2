namespace Services;

using Models;
public interface IAuthService
{
    public string Login(UserAuth user);
    public string Register(UserRegisterPassword user);
    public string GenerateToken(UserToken userToken);

    


}