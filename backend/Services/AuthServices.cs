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
using System.Security.Cryptography;

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
        var salt = GenerateSalt();
        var passwordHash = HashPassword(userAuth.Password, salt);
        userAuth.Password = passwordHash;

        var user = _repository.GetUserFromCredentials(userAuth);
        if (user == null)
        {
            return "";
        }

        var userToken = new UserToken
        {
            Id = user.Id,
            Role = user.Role
        };
        return GenerateToken(userToken);
    }

    public string Register(UserRegisterPassword userRegister)
    {
        var salt = GenerateSalt();
        var passwordHash = HashPassword(userRegister.Password, salt);
        var user = new User
        {
            Name = userRegister.Name,
            Username = userRegister.Username,
            Email = userRegister.Email,
            Password = passwordHash,
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

    public byte[] GenerateSalt()
    {
        return Encoding.UTF8.GetBytes("Admin1234");
    }

    public string HashPassword(string password, byte[] salt)
    {
        int keySize = 64;
        int iterations = 1000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password),
                                             salt,
                                             iterations,
                                             hashAlgorithm,
                                             keySize);

        return Convert.ToHexString(hash);
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
}

