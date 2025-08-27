namespace Services;

using Models;

public interface IPlaylistService
{
    Task<List<Playlist>> GetAllAsync();
    Task<Playlist?> GetByIdAsync(int id);
    Task AddAsync(PlaylistCreate playlist);
    Task UpdateAsync(PlaylistCreate playlist, int id);
    Task DeleteAsync(int id);
}