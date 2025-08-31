using Models;

namespace Data;

public interface IArtistEfRepository
{
    Task<List<Artist>> GetAllAsync(ArtistDtoParameters artistDtoParameters);
    Task<Artist?> GetByIdAsync(int id);
    Task AddAsync(Artist artist);
    Task UpdateAsync(Artist artist);
    Task<bool> DeleteAsync(int id);
    Task<Artist?> GetAlbumsByArtist(int id);

}