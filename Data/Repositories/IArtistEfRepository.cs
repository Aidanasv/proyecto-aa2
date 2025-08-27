using Models;

namespace Data;

public interface IArtistEfRepository
{
    Task<List<Artist>> GetAllAsync();

    Task<Artist?> GetByIdAsync(int id);
    Task AddAsync(Artist artist);
    Task UpdateAsync(Artist artist);
    Task<bool> DeleteAsync(int id);

}