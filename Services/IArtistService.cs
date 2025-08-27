namespace Services;

using Models;

public interface IArtistService
{
    Task<List<Artist>> GetAllAsync();
    Task<Artist?> GetByIdAsync(int id);
    Task AddAsync(ArtistCreate artist);
    Task UpdateAsync(ArtistCreate artist, int id);
    Task DeleteAsync(int id);
}