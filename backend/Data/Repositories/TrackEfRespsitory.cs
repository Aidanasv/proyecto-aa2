namespace Data;

using Microsoft.EntityFrameworkCore;
using Models;

public class TrackEfRepository : ITrackEfRepository
{
    private readonly MusicDbContext _dbContext;

    public TrackEfRepository(MusicDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Track>> GetAllAsync()
    {
        return await _dbContext.Tracks.ToListAsync();
    }

    public async Task<Track?> GetByIdAsync(int id)
    {
        return await _dbContext.Tracks.FindAsync(id);
    }

    public async Task AddAsync(Track track)
    {
        await _dbContext.Tracks.AddAsync(track);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Track track)
    {
        _dbContext.Tracks.Update(track);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var track = await _dbContext.Tracks.FindAsync(id);
        if (track == null) return false;

        _dbContext.Tracks.Remove(track);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}