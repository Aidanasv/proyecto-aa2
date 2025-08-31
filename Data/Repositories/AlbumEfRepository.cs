using Microsoft.EntityFrameworkCore;
using Models;

namespace Data;

public class AlbumEfRepository : IAlbumEfRepository
{
    private readonly MusicDbContext _dbContext;

    public AlbumEfRepository(MusicDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Album>> GetAllAsync()
    {
        return await _dbContext.Albums.ToListAsync();
    }

    public async Task<Album?> GetByIdAsync(int id)
    {
        return await _dbContext.Albums.FindAsync(id);
    }

    public async Task AddAsync(Album album)
    {
        await _dbContext.Albums.AddAsync(album);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Album album)
    {
        _dbContext.Albums.Update(album);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var album = await _dbContext.Albums.FindAsync(id);
        if (album == null) return false;

        _dbContext.Albums.Remove(album);
        await _dbContext.SaveChangesAsync();
        return true;
    }
    
    public async Task<Album?> GetTracksByAlbum(int id)
    {
        return _dbContext.Albums.Include(album => album.Tracks).FirstOrDefault(album => album.Id == id);
    }

}