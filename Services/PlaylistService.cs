namespace Services;

using Data;
using Models;

public class PlaylistService : IPlaylistService
{
    private readonly IPlaylistEfRepository _playlistRepository;

    public PlaylistService(IPlaylistEfRepository playlistRepository)
    {
        _playlistRepository = playlistRepository;
    }

    public async Task<List<Playlist>> GetAllAsync()
    {
        return await _playlistRepository.GetAllAsync();
    }

    public async Task<Playlist?> GetByIdAsync(int id)
    {
        return await _playlistRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(PlaylistCreate playlistCreate)
    {
        var playlist = new Playlist
        {
            Name = playlistCreate.Name,
            Description = playlistCreate.Description,
            UserId = playlistCreate.UserId,
            SoftDelete = playlistCreate.SoftDelete
        };

        await _playlistRepository.AddAsync(playlist);
    }

    public async Task UpdateAsync(PlaylistCreate playlist, int id)
    {
        var updatedPlaylist = await _playlistRepository.GetByIdAsync(id);
        if (updatedPlaylist == null)
        {
            throw KeyNotFoundException("Playlis no encontrada");
        }

        updatedPlaylist.Name = playlist.Name;
        updatedPlaylist.Description = playlist.Description;
        

        await _playlistRepository.UpdateAsync(updatedPlaylist);
    }

    public async Task DeleteAsync(int id)
    {
        var playlist = await _playlistRepository.GetByIdAsync(id);
        if (playlist == null)
        {
            throw KeyNotFoundException("Playlist no encontrado");
        }
        playlist.SoftDelete = true;

        await _playlistRepository.UpdateAsync(playlist);
    }

    private Exception KeyNotFoundException(string v)
    {
        throw new NotImplementedException();
    }
}