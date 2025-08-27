namespace Services;

using System;
using Data;
using Models;

public class TrackService : ITrackService
{
    private readonly ITrackEfRepository _trackRepository;

    public TrackService(ITrackEfRepository trackRepository)
    {
        _trackRepository = trackRepository;
    }

    public async Task<List<Track>> GetAllAsync()
    {
        return await _trackRepository.GetAllAsync();
    }

    public async Task<Track?> GetByIdAsync(int id)
    {
        return await _trackRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(TrackCreate trackCreate)
    {
        var track = new Track
        {
            Name = trackCreate.Name,
            ArtistId = trackCreate.ArtistId,
            AlbumId = trackCreate.AlbumId,
            Duration = trackCreate.Duration,
            Link = trackCreate.Link,
            Plays = trackCreate.Plays,
            ReleaseDate = trackCreate.ReleaseDate
        };

        await _trackRepository.AddAsync(track);
    }

    public async Task UpdateAsync(TrackCreate track, int id)
    {
        var updatedTrack = await _trackRepository.GetByIdAsync(id);
        if (updatedTrack == null)
        {
            throw KeyNotFoundException("Canción no encontrada");
        }

        updatedTrack.Name = track.Name;
        updatedTrack.Duration = track.Duration;
        updatedTrack.ReleaseDate = track.ReleaseDate;
        updatedTrack.Link = track.Link;
        updatedTrack.Plays = track.Plays;

        await _trackRepository.UpdateAsync(updatedTrack);
    }

    public async Task DeleteAsync(int id)
    {
        var track = await _trackRepository.GetByIdAsync(id);
        if (track == null)
        {
            throw KeyNotFoundException("Canción no encontrada");
        }
        track.SoftDelete = true;

        await _trackRepository.UpdateAsync(track);
    }

    private Exception KeyNotFoundException(string v)
    {
        throw new NotImplementedException();
    }
}