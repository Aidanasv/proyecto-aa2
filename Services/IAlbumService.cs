using Models;

namespace Services;

public interface IAlbumService
{
    Task<List<Album>> GetAllAsync();
    Task<Album?> GetByIdAsync(int id);
    Task<Album> AddAsync(AlbumCreate album);
    Task<Album> UpdateAsync(AlbumCreate album, int id);
    Task DeleteAsync(int id);
    Task<AlbumTrackDTO?> GetTracksByAlbum(int id);
}