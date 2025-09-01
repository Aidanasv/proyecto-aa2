namespace Services;

using Data;
using Models;

public class UserService : IUserService
{
    private readonly IUserEfRepository _userRepository;

    public UserService(IUserEfRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(UserCreate userCreate)
    {
        var user = new User
        {
            Name = userCreate.Name,
            Email = userCreate.Email,
            Password = userCreate.Password,
            Username = userCreate.Username,
            BirthDate = userCreate.BirthDate,
            CreateDate = userCreate.CreateDate,
            LastLogin = userCreate.LastLogin,
            Role = userCreate.Role
        };

        await _userRepository.AddAsync(user);
    }

    public async Task UpdateAsync(UserCreate user, int id)
    {
        var updatedUser = await _userRepository.GetByIdAsync(id);
        if (updatedUser == null)
        {
            throw KeyNotFoundException("Usuario no encontrado");
        }

        updatedUser.Name = user.Name;
        updatedUser.Username = user.Username;
        updatedUser.BirthDate = user.BirthDate;
        updatedUser.Email = user.Email;
        updatedUser.Password = user.Password;

        await _userRepository.UpdateAsync(updatedUser);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw KeyNotFoundException("Usuario no encontrado");
        }

        await _userRepository.DeleteAsync(id);
    }

    private Exception KeyNotFoundException(string v)
    {
        throw new NotImplementedException();
    }
}