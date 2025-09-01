using Models;

namespace Data;

public interface IPlaylistEfRepository
{
    Task<List<Playlist>> GetAllAsync();

    Task<Playlist?> GetByIdAsync(int id);
    Task AddAsync(Playlist playlist);
    Task UpdateAsync(Playlist playlist);
    Task<bool> DeleteAsync(int id);
    Task<List<Playlist>> GetPlaylistsByUser(PlaylistDtoParameters playlistDtoParameters,int idUser);

}