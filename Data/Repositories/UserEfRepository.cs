namespace Data;

using Microsoft.EntityFrameworkCore;
using Models;

public class UserEfRepository : IUserEfRepository
{
    private readonly MusicDbContext _dbContext;

    public UserEfRepository(MusicDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _dbContext.Users.FindAsync(id);
    }

    public async Task AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null) return false;

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public UserToken? GetUserFromCredentials(UserAuth userAuth)
    {
        var user = _dbContext.Users.FirstOrDefault(userDb => userDb.Email == userAuth.Email && userDb.Password == userAuth.Password);

        var userToken = new UserToken
        {
            Id = user.Id,
            Role = user.Role
        };
        return userToken;
    }

}