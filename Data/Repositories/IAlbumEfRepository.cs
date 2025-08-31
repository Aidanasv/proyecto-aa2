namespace Data;

using Models;

public interface IAlbumEfRepository
{
    Task<List<Album>> GetAllAsync();

    Task<Album?> GetByIdAsync(int id);
    Task AddAsync(Album album);
    Task UpdateAsync(Album album);
    Task<bool> DeleteAsync(int id);
    Task<Album?> GetTracksByAlbum(int id);
}