using Models;

namespace Services;

public interface IAlbumService
{
    Task<List<Album>> GetAllAsync();
    Task<Album?> GetByIdAsync(int id);
    Task AddAsync(AlbumCreate album);
    Task UpdateAsync(AlbumCreate album, int id);
    Task DeleteAsync(int id);
}