namespace Services;

using Models;

public interface IUserService
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task AddAsync(UserCreate user);
    Task UpdateAsync(UserCreate user, int id);
    Task DeleteAsync(int id);
}