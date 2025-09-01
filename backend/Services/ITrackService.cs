namespace Services;

using Models;

public interface ITrackService
{
    Task<List<Track>> GetAllAsync();
    Task<Track?> GetByIdAsync(int id);
    Task<Track> AddAsync(TrackCreate track);
    Task<Track> UpdateAsync(TrackCreate track, int id);
    Task DeleteAsync(int id);
    Task<byte[]> GetAudio(int id);

}