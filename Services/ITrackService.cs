namespace Services;

using Models;

public interface ITrackService
{
    Task<List<Track>> GetAllAsync();
    Task<Track?> GetByIdAsync(int id);
    Task AddAsync(TrackCreate track);
    Task UpdateAsync(TrackCreate track, int id);
    Task DeleteAsync(int id);
}