namespace Services;

using Models;

public interface IPlaylistService
{
    Task<List<Playlist>> GetAllAsync();
    Task<PlaylistDto?> GetByIdAsync(int id);
    Task<PlaylistRead> AddAsync(PlaylistCreate playlist, int UserId);
    Task<PlaylistRead> UpdateAsync(PlaylistCreate playlist, int id);
    Task DeleteAsync(int id);
    Task<PlaylistDto> AddTrackToPlaylist(int id, int idTrack);
    Task<PlaylistDto> DeleteTrackToPlaylist(int id, int idTrack);
}