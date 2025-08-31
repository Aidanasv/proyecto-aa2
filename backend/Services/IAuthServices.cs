namespace Services;

using Models;
public interface IAuthService
{
    public string Login(UserAuth user);
    //public string Register(UserCreate user);
    public string GenerateToken(UserToken userToken);
    //public bool HasAccessToResource(int requestedUserID, ClaimsPrincipal user);


}