namespace Data;

using Models;

public interface ITrackEfRepository
{
    Task<List<Track>> GetAllAsync();
    Task<Track?> GetByIdAsync(int id);
    Task AddAsync(Track track);
    Task UpdateAsync(Track track);
    Task<bool> DeleteAsync(int id);

}