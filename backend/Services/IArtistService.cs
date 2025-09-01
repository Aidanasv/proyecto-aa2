namespace Services;

using Models;

public interface IArtistService
{
    Task<List<Artist>> GetAllAsync(ArtistDtoParameters artistDtoParameters);
    Task<Artist?> GetByIdAsync(int id);
    Task<Artist> AddAsync(ArtistCreate artist);
    Task<Artist> UpdateAsync(ArtistCreate artist, int id);
    Task DeleteAsync(int id);
    Task<ArtistDto?> GetAlbumsByArtist(int id);
}