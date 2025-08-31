namespace Data;

using Microsoft.EntityFrameworkCore;
using Models;

public class PlaylistEfRepository : IPlaylistEfRepository
{
    private readonly MusicDbContext _dbContext;

    public PlaylistEfRepository(MusicDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Playlist>> GetAllAsync()
    {
        return await _dbContext.Playlists.ToListAsync();
    }

    public async Task<Playlist?> GetByIdAsync(int id)
    {
        return _dbContext.Playlists.Include(playlist => playlist.Tracks).FirstOrDefault(playlist => playlist.Id == id);
    }

    public async Task AddAsync(Playlist playlist)
    {
        await _dbContext.Playlists.AddAsync(playlist);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Playlist playlist)
    {
        _dbContext.Playlists.Update(playlist);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var playlist = await _dbContext.Playlists.FindAsync(id);
        if (playlist == null) return false;

        _dbContext.Playlists.Remove(playlist);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<Playlist>> GetPlaylistsByUser(int idUser)
    {
        var playlist = await _dbContext.Playlists.Include(playlist => playlist.Tracks).Where(playlist => playlist.UserId == idUser && playlist.SoftDelete == false).ToListAsync();
        return playlist;
    }


}