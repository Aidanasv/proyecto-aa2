namespace Data;

using Microsoft.EntityFrameworkCore;
using Models;

public class ArtistEfRepository : IArtistEfRepository
{
    private readonly MusicDbContext _dbContext;

    public ArtistEfRepository(MusicDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Artist>> GetAllAsync()
    {
        return await _dbContext.Artists.ToListAsync();
    }

    public async Task<Artist?> GetByIdAsync(int id)
    {
        return await _dbContext.Artists.FindAsync(id);
    }

    public async Task AddAsync(Artist artist)
    {
        await _dbContext.Artists.AddAsync(artist);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Artist artist)
    {
        _dbContext.Artists.Update(artist);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var artist = await _dbContext.Artists.FindAsync(id);
        if (artist == null) return false;

        _dbContext.Artists.Remove(artist);
        await _dbContext.SaveChangesAsync();
        return true;
    }

}