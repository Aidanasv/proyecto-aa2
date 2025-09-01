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

    public async Task<List<Playlist>> GetPlaylistsByUser(PlaylistDtoParameters playlistDtoParameters,int idUser)
    {

        var query = _dbContext.Playlists.Include(playlist => playlist.Tracks).Where(playlist => playlist.UserId == idUser && playlist.SoftDelete == false);


        if (playlistDtoParameters.Name != null || playlistDtoParameters.Description != null)
        {
            query = query.Where(
            playlist => (
                (playlistDtoParameters.Name != null && playlist.Name.ToLower().Contains(playlistDtoParameters.Name.ToLower()))
                 || (playlistDtoParameters.Description != null && playlist.Description.ToLower().Contains(playlistDtoParameters.Description.ToLower()))));


        }
        
        IOrderedQueryable<Playlist>? orderedQuery = null;

        if (playlistDtoParameters.NameOrder == true)
        {
            orderedQuery = query.OrderBy(playlist => playlist.Name);
        }
        else if (playlistDtoParameters.NameOrder == false)
        {
            orderedQuery = query.OrderByDescending(playlist => playlist.Name);
        }

        if (playlistDtoParameters.DescriptionOrder == true)
        {
            orderedQuery = orderedQuery == null
                ? query.OrderBy(playlist => playlist.Description)
                : orderedQuery.ThenBy(playlist => playlist.Description);
        }
        else if (playlistDtoParameters.DescriptionOrder == false)
        {
            orderedQuery = orderedQuery == null
                ? query.OrderByDescending(playlist => playlist.Description)
                : orderedQuery.ThenByDescending(playlist => playlist.Description);
        }
        var playlist = await (orderedQuery ?? query).ToListAsync();
        return playlist;
    }


}