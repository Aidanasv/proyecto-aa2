namespace Services;

using Data;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserEfRepository _repository;

    public AuthService(IConfiguration configuration, IUserEfRepository repository)
    {
        _configuration = configuration;
        _repository = repository;
    }

    public string Login(UserAuth userAuth)
    {
        var user = _repository.GetUserFromCredentials(userAuth);
        if (user == null)
        {
            return "";
        }
        return GenerateToken(user);
    }

    public string Register(UserRegister userRegister)
    {
        

        var user = new User
        {
            Name = userRegister.Name,
            Username = userRegister.Username,
            Email = userRegister.Email,
            Password = userRegister.Password,
            BirthDate = userRegister.BirthDate,
            Role = Role.Client
        };

        _repository.AddAsync(user);

        var userToken = new UserToken
        {
            Id = user.Id,
            Role = user.Role
        };

        return GenerateToken(userToken);
    }

    public string GenerateToken(UserToken userToken)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["JWT:ValidIssuer"],
            Audience = _configuration["JWT:ValidAudience"],
            Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.NameIdentifier, Convert.ToString(userToken.Id)),
                        new Claim(ClaimTypes.Role, userToken.Role),
                }),
            Expires = DateTime.UtcNow.AddDays(7), // AddMinutes(60)
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        return tokenString;
    }
/*     public bool HasAccessToResource(int requestedUserID, ClaimsPrincipal user)
    {
        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return false;
        }
        var isOwnResource = userId == requestedUserID;

        var roleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        if (roleClaim != null) return false;
        var isAdmin = roleClaim!.Value == Roles.Admin;

        var hasAccess = isOwnResource || isAdmin;
        return hasAccess;
    }
  */

}

