using Models;

namespace Data;

public interface IUserEfRepository
{
    Task<List<User>> GetAllAsync();

    Task<User?> GetByIdAsync(int id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task<bool> DeleteAsync(int id);
}